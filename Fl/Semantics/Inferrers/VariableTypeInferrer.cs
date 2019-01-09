// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Exceptions;
using Fl.Semantics.Types;

namespace Fl.Semantics.Inferrers
{
    class VariableTypeInferrer : INodeVisitor<TypeInferrerVisitor, VariableNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, VariableNode vardecl)
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

        protected InferredType VarDefinitionNode(TypeInferrerVisitor visitor, VariableDefinitionNode vardecl)
        {
            foreach (var definition in vardecl.Definitions)
            {
                // Symbol should be already resolved here
                var leftSymbol = visitor.SymbolTable.Get(definition.Left.Value);

                // If the rhs is null, continue, is just a declaration
                if (definition.Right == null)
                    continue;

                // If it is a variable definition, get the right-hand side type info
                var rhsInferredType = definition.Right?.Visit(visitor);

                // If the symbol is an anonymous type, the rhs type is a must
                if (leftSymbol.TypeInfo.IsAnonymousType && (rhsInferredType == null || rhsInferredType.TypeInfo == null || rhsInferredType.TypeInfo.Type == null))
                    throw new SymbolException($"Implicitly-typed variable '{leftSymbol.Name}' needs to be initialized");

                // Get the most general type that encloses both types
                var generalType = visitor.Inferrer.FindMostGeneralType(leftSymbol.TypeInfo, rhsInferredType.TypeInfo);

                // Update lhs and rhs (if present)
                visitor.Inferrer.Unify(generalType, leftSymbol.TypeInfo, rhsInferredType.Symbol?.TypeInfo);
            }

            return null;
        }

        protected InferredType VarDestructuringNode(TypeInferrerVisitor visitor, VariableDestructuringNode destructuringNode)
        {
            var inferredType = destructuringNode.Right.Visit(visitor);

            for (int i=0; i < destructuringNode.Left.Count; i++)
            {
                var declaration = destructuringNode.Left[i];

                if (declaration == null)
                    continue;

                // Symbol should be already resolved here
                var lhs = visitor.SymbolTable.Get(declaration.Value);

                // If it is a variable definition, get the right-hand side type info
                var rhsType = (inferredType.TypeInfo.Type as Tuple).Types[i];

                // Check types to see if we can unify them
                visitor.Inferrer.FindMostGeneralType(lhs.TypeInfo, rhsType);
            }

            return inferredType;
        }
    }
}
