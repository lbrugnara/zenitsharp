// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Types;

namespace Fl.Semantics.Checkers
{
    class AssignmentTypeChecker : INodeVisitor<TypeCheckerVisitor, AstAssignmentNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, AstAssignmentNode node)
        {
            if (node is AstVariableAssignmentNode)
                return MakeVariableAssignment(node as AstVariableAssignmentNode, checker);
            if (node is AstDestructuringAssignmentNode)
                return this.MakeDestructuringAssignment(node as AstDestructuringAssignmentNode, checker);

            throw new AstWalkerException($"Invalid variable assignment of type {node.GetType().FullName}");
        }

        private CheckedType MakeVariableAssignment(AstVariableAssignmentNode node, TypeCheckerVisitor checker)
        {
            if (node.Accessor.Enclosing != null)
            {
                var enc = node.Accessor.Enclosing.Visit(checker);

                if (enc.Type is Class c)
                    throw new System.Exception($"An instance of {c.Name} '{c.ClassName}' is required to access member '{node.Accessor.Identifier.Value}'");
            }

            var leftHandSide = node.Accessor.Visit(checker);
            var rightHandSide = node.Expression.Visit(checker);

            if (leftHandSide.Symbol.Storage == Symbols.Storage.Constant)
                throw new System.Exception($"Cannot change value of constant {leftHandSide.Type.Name} '{leftHandSide.Symbol.Name}'");

            if (!leftHandSide.Type.IsAssignableFrom(rightHandSide.Type))
                throw new System.Exception($"Cannot convert from {rightHandSide.Type} to {leftHandSide.Type}");

            leftHandSide.Symbol = null;

            return leftHandSide;
        }

        private CheckedType MakeDestructuringAssignment(AstDestructuringAssignmentNode node, TypeCheckerVisitor checker)
        {
            var tupleCheckedType = node.Variables.Visit(checker);
            var exprCheckedType = node.Expression.Visit(checker);

            var tupleTypes = tupleCheckedType.Type as Tuple;
            var exprTypes = exprCheckedType.Type as Tuple;

            for (int i = 0; i < tupleTypes.Count; i++)
            {
                var varType = tupleTypes.Types[i];

                if (varType == null)
                    continue;

                var varnode = node.Variables.Items[i];

                if (varnode is AstAccessorNode accessor && accessor.Enclosing != null)
                {
                    var enc = accessor.Enclosing.Visit(checker);

                    if (enc.Type is Class c)
                        throw new System.Exception($"An instance of {c.Name} '{c.ClassName}' is required to access member '{accessor.Identifier.Value}'");
                }

                var leftHandSide = varnode.Visit(checker);

                if (leftHandSide.Symbol.Storage == Symbols.Storage.Constant)
                    throw new System.Exception($"Cannot change value of constant {leftHandSide.Type.Name} '{leftHandSide.Symbol.Name}'");

                var exprType = exprTypes.Types[i];

                if (!varType.IsAssignableFrom(exprType))
                    throw new System.Exception($"Cannot convert from {varType} to {exprType}");
            }

            return exprCheckedType;
        }
    }
}
