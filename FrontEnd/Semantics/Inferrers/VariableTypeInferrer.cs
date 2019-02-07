// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Exceptions;
using Zenit.Semantics.Symbols;
using Zenit.Semantics.Symbols.Types;
using Zenit.Semantics.Symbols.Types.Specials;
using Zenit.Semantics.Symbols.Values;

namespace Zenit.Semantics.Inferrers
{
    class VariableTypeInferrer : INodeVisitor<TypeInferrerVisitor, VariableNode, ITypeSymbol>
    {
        public ITypeSymbol Visit(TypeInferrerVisitor visitor, VariableNode vardecl)
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

        protected ITypeSymbol VarDefinitionNode(TypeInferrerVisitor visitor, VariableDefinitionNode vardecl)
        {
            foreach (var definition in vardecl.Definitions)
            {
                // Symbol should be already resolved here
                var leftSymbol = visitor.SymbolTable.GetBoundSymbol(definition.Left.Value);

                // If the rhs is null, continue, is just a declaration
                if (definition.Right == null)
                    continue;

                // If it is a variable definition, get the right-hand side type info
                var rhsTypeSymbol = definition.Right?.Visit(visitor);

                // If the symbol is an anonymous type, the rhs type is a must
                if (leftSymbol.TypeSymbol is AnonymousSymbol && rhsTypeSymbol == null)
                    throw new SymbolException($"Implicitly-typed variable '{leftSymbol.Name}' needs to be initialized");

                // Get the most general type that encloses both types
                var generalType = visitor.Inferrer.FindMostGeneralType(leftSymbol.TypeSymbol, rhsTypeSymbol);

                // Update lhs and rhs (if present)
                visitor.Inferrer.Unify(visitor.SymbolTable, generalType, leftSymbol);
            }

            return null;
        }

        protected ITypeSymbol VarDestructuringNode(TypeInferrerVisitor visitor, VariableDestructuringNode destructuringNode)
        {
            var rhsTupleType = destructuringNode.Right.Visit(visitor) as TupleSymbol;

            for (int i=0; i < destructuringNode.Left.Count; i++)
            {
                var declaration = destructuringNode.Left[i];

                if (declaration == null)
                    continue;

                // Symbol should be already resolved here
                var lhs = visitor.SymbolTable.GetBoundSymbol(declaration.Value);

                // If it is a variable definition, get the right-hand side type info
                var rhsType = rhsTupleType.Types[i];

                // Check types to see if we can unify them
                var generalType = visitor.Inferrer.FindMostGeneralType(lhs.TypeSymbol, rhsType is ITypeSymbol rts ? rts : (rhsType as IBoundSymbol).TypeSymbol);

                visitor.Inferrer.Unify(visitor.SymbolTable, generalType, lhs as IBoundSymbol);
            }

            return rhsTupleType;
        }
    }
}
