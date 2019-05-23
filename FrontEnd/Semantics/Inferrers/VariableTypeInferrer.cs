// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Exceptions;
using Zenit.Semantics.Symbols;
using Zenit.Semantics.Symbols.Types;
using Zenit.Semantics.Symbols.Types.References;
using Zenit.Semantics.Symbols.Types.Specials;
using Zenit.Semantics.Symbols.Variables;

namespace Zenit.Semantics.Inferrers
{
    class VariableTypeInferrer : INodeVisitor<TypeInferrerVisitor, VariableNode, IType>
    {
        public IType Visit(TypeInferrerVisitor visitor, VariableNode vardecl)
        {
            switch (vardecl)
            {
                case VariableDefinitionNode vardefnode:
                    return VarDefinitionNode(visitor, vardefnode);

                case VariableDestructuringNode vardestnode:
                    return VarDestructuringNode(visitor, vardestnode);
            }
            throw new AstWalkerException($"Invalid variable declaration of type {vardecl.GetType().FullName}");
        }

        protected IType VarDefinitionNode(TypeInferrerVisitor visitor, VariableDefinitionNode vardecl)
        {
            foreach (var definition in vardecl.Definitions)
            {
                // Symbol should be already resolved here
                var leftSymbol = visitor.SymbolTable.GetVariableSymbol(definition.Left.Value);

                // If the rhs is null, continue, is just a declaration
                if (definition.Right == null)
                    continue;

                // If it is a variable definition, get the right-hand side type info
                var rhsTypeSymbol = definition.Right?.Visit(visitor);

                // If the symbol is an anonymous type, the rhs type is a must
                if (leftSymbol.TypeSymbol is Anonymous && rhsTypeSymbol == null)
                    throw new SymbolException($"Implicitly-typed variable '{leftSymbol.Name}' needs to be initialized");

                // Get the most general type that encloses both types
                var generalType = visitor.Inferrer.FindMostGeneralType(leftSymbol.TypeSymbol, rhsTypeSymbol);

                // Update lhs and rhs (if present)
                visitor.Inferrer.Unify(visitor.SymbolTable, generalType, leftSymbol);
            }

            return null;
        }

        protected IType VarDestructuringNode(TypeInferrerVisitor visitor, VariableDestructuringNode destructuringNode)
        {
            var rhsTupleType = destructuringNode.Right.Visit(visitor) as Tuple;

            for (int i=0; i < destructuringNode.Left.Count; i++)
            {
                var declaration = destructuringNode.Left[i];

                if (declaration == null)
                    continue;

                // Symbol should be already resolved here
                var lhs = visitor.SymbolTable.GetVariableSymbol(declaration.Value);

                // If it is a variable definition, get the right-hand side type info
                var rhsType = rhsTupleType.Elements[i];

                // Check types to see if we can unify them
                var generalType = visitor.Inferrer.FindMostGeneralType(lhs.TypeSymbol, rhsType is IType rts ? rts : (rhsType as IVariable).TypeSymbol);

                visitor.Inferrer.Unify(visitor.SymbolTable, generalType, lhs as IVariable);
            }

            return rhsTupleType;
        }
    }
}
