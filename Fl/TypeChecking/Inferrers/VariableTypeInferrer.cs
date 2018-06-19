// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Symbols.Types;

namespace Fl.TypeChecking.Inferrers
{
    class VariableTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstVariableNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, AstVariableNode vardecl)
        {
            switch (vardecl)
            {
                case AstVarDefinitionNode vardefnode:
                    return VarDefinitionNode(visitor, vardefnode);

                case AstVarDestructuringNode vardestnode:
                    return VarDestructuringNode(visitor, vardestnode);
            }
            throw new AstWalkerException($"Invalid variable declaration of type {vardecl.GetType().FullName}");
        }

        protected InferredType VarDefinitionNode(TypeInferrerVisitor visitor, AstVarDefinitionNode vardecl)
        {
            // Get the variable type from the declaration
            InferredType inferredType = null;

            foreach (var declaration in vardecl.VarDefinitions)
            {
                // Symbol should be already resolved here
                var lhs = visitor.SymbolTable.GetSymbol(declaration.Item1.Value.ToString());

                if (inferredType == null)
                    inferredType = new InferredType(lhs.Type);

                // If the rhs is null, continue, is just a declaration
                if (declaration.Item2 == null)
                    continue;

                // If it is a variable definition, get the right-hand side type info
                var rhs = declaration.Item2?.Visit(visitor);

                // Check types to see if we can unify them
                visitor.Inferrer.MakeConclusion(lhs.Type, rhs.Type);
            }

            return inferredType;
        }

        protected InferredType VarDestructuringNode(TypeInferrerVisitor visitor, AstVarDestructuringNode vardestnode)
        {
            var initType = vardestnode.DestructInit.Visit(visitor);

            // Get the variable type from the declaration
            InferredType inferredType = new InferredType(initType.Type);

            for (int i=0; i < vardestnode.Variables.Count; i++)
            {
                var declaration = vardestnode.Variables[i];

                // Symbol should be already resolved here
                var lhs = visitor.SymbolTable.GetSymbol(declaration.Value.ToString());

                // If it is a variable definition, get the right-hand side type info
                var rhsType = (initType.Type as Tuple).Types[i];

                // Check types to see if we can unify them
                visitor.Inferrer.MakeConclusion(lhs.Type, rhsType);
            }

            return inferredType;
        }
    }
}
