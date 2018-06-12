// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Ast;
using Fl.Lang.Types;

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

                // Assign a temporal type if it is not present
                if (lhs.Type == null)
                    visitor.Inferrer.AssumeSymbolType(lhs);

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

        protected InferredType VarDestructuringNode(TypeInferrerVisitor checker, AstVarDestructuringNode vardestnode)
        {
            // Get the variable type
            //TypeResolver typeresolver = TypeResolver.GetTypeResolverFromToken(vardestnode.VarType.TypeToken);

            /*vardestnode.DestructInit.Exec()

            foreach (var declaration in vardestnode.VarDefinitions)
            {
                var identifierToken = declaration.Item1;
                var initializerInstr = declaration.Item2?.Exec(checker);

                checker.Emmit(new LocalVarInstruction(dataType, identifierToken.Value.ToString(), initializerInstr?.TargetName));
            }*/
            return null;
        }
    }
}
