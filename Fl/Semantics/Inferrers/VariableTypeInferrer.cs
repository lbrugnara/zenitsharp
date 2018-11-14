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
            // Get the variable type from the declaration
            InferredType inferredType = null;

            foreach (var definition in vardecl.Definitions)
            {
                // Symbol should be already resolved here
                var lhs = visitor.SymbolTable.GetSymbol(definition.Left.Value);

                if (inferredType == null)
                    inferredType = new InferredType(lhs.Type);

                // If the rhs is null, continue, is just a declaration
                if (definition.Right == null)
                    continue;

                // If it is a variable definition, get the right-hand side type info
                var rhs = definition.Right?.Visit(visitor);

                if (visitor.Inferrer.IsTypeAssumption(lhs.Type) && (rhs == null || rhs.Type == null))
                    throw new SymbolException($"Implicitly-typed variable '{lhs.Name}' needs to be initialized");

                // Check types to see if we can unify them
                visitor.Inferrer.MakeConclusion(lhs.Type, rhs.Type);
            }

            return inferredType;
        }

        protected InferredType VarDestructuringNode(TypeInferrerVisitor visitor, VariableDestructuringNode destructuringNode)
        {
            var initType = destructuringNode.Right.Visit(visitor);

            // Get the variable type from the declaration
            InferredType inferredType = new InferredType(initType.Type);

            for (int i=0; i < destructuringNode.Left.Count; i++)
            {
                var declaration = destructuringNode.Left[i];

                if (declaration == null)
                    continue;

                // Symbol should be already resolved here
                var lhs = visitor.SymbolTable.GetSymbol(declaration.Value);

                // If it is a variable definition, get the right-hand side type info
                var rhsType = (initType.Type as Tuple).Types[i];

                // Check types to see if we can unify them
                visitor.Inferrer.MakeConclusion(lhs.Type, rhsType);
            }

            return inferredType;
        }
    }
}
