// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Symbols.Types;

namespace Fl.TypeChecking.Inferrers
{
    class VariableTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstVariableNode, Type>
    {
        public Type Visit(TypeInferrerVisitor visitor, AstVariableNode vardecl)
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

        protected Type VarDefinitionNode(TypeInferrerVisitor visitor, AstVarDefinitionNode vardecl)
        {
            // Get the variable type from the declaration
            Type inferredType = null;

            foreach (var declaration in vardecl.VarDefinitions)
            {
                // Symbol should be already resolved here
                var lhs = visitor.SymbolTable.GetSymbol(declaration.Item1.Value.ToString());

                // Assign a temporal type if it is not present
                if (lhs.DataType == null)
                    visitor.Inferrer.AssumeSymbolType(lhs);

                if (inferredType == null)
                    inferredType = lhs.DataType;

                // If the rhs is null, continue, is just a declaration
                if (declaration.Item2 == null)
                    continue;

                // If it is a variable definition, get the right-hand side type info
                var rhs = declaration.Item2?.Visit(visitor);

                // Check types to see if we can unify them
                visitor.Inferrer.MakeConclusion(lhs.DataType, rhs.DataType);
            }

            return inferredType;
        }

        protected Type VarDestructuringNode(TypeInferrerVisitor checker, AstVarDestructuringNode vardestnode)
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
