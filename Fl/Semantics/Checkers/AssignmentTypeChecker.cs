// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Types;

namespace Fl.Semantics.Checkers
{
    class AssignmentTypeChecker : INodeVisitor<TypeCheckerVisitor, AssignmentNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, AssignmentNode node)
        {
            if (node is VariableAssignmentNode)
                return MakeVariableAssignment(node as VariableAssignmentNode, checker);
            if (node is DestructuringAssignmentNode)
                return this.MakeDestructuringAssignment(node as DestructuringAssignmentNode, checker);

            throw new AstWalkerException($"Invalid variable assignment of type {node.GetType().FullName}");
        }

        private CheckedType MakeVariableAssignment(VariableAssignmentNode node, TypeCheckerVisitor checker)
        {
            if (node.Accessor.Parent != null)
            {
                var enc = node.Accessor.Parent.Visit(checker);

                /*if (enc.TypeInfo.Type is Class c)
                    throw new System.Exception($"An instance of {c.Name} '{c.ClassName}' is required to access member '{node.Accessor.Target.Value}'");*/
            }

            var leftHandSide = node.Accessor.Visit(checker);
            var rightHandSide = node.Right.Visit(checker);

            if (leftHandSide.Symbol.Storage == Symbols.Storage.Constant)
                throw new System.Exception($"Cannot change value of constant {leftHandSide.TypeInfo.Type.Name} '{leftHandSide.Symbol.Name}'");

            if (!leftHandSide.TypeInfo.Type.IsAssignableFrom(rightHandSide.TypeInfo.Type))
                throw new System.Exception($"Cannot convert from {rightHandSide.TypeInfo} to {leftHandSide.TypeInfo}");

            leftHandSide.Symbol = null;

            return leftHandSide;
        }

        private CheckedType MakeDestructuringAssignment(DestructuringAssignmentNode node, TypeCheckerVisitor checker)
        {
            var tupleCheckedType = node.Left.Visit(checker);
            var exprCheckedType = node.Right.Visit(checker);

            var tupleTypes = tupleCheckedType.TypeInfo.Type as Tuple;
            var exprTypes = exprCheckedType.TypeInfo.Type as Tuple;

            for (int i = 0; i < tupleTypes.Count; i++)
            {
                var varType = tupleTypes.Types[i];

                if (varType == null)
                    continue;

                var varnode = node.Left.Items[i];

                if (varnode is AccessorNode accessor && accessor.Parent != null)
                {
                    var enc = accessor.Parent.Visit(checker);

                    /*if (enc.TypeInfo.Type is Class c)
                        throw new System.Exception($"An instance of {c.Name} '{c.ClassName}' is required to access member '{accessor.Target.Value}'");*/
                }

                var leftHandSide = varnode.Visit(checker);

                if (leftHandSide.Symbol.Storage == Symbols.Storage.Constant)
                    throw new System.Exception($"Cannot change value of constant {leftHandSide.TypeInfo.Type.Name} '{leftHandSide.Symbol.Name}'");

                var exprType = exprTypes.Types[i];

                if (!varType.IsAssignableFrom(exprType))
                    throw new System.Exception($"Cannot convert from {varType} to {exprType}");
            }

            return exprCheckedType;
        }
    }
}
