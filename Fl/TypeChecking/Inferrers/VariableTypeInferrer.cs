// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Ast;
using Fl.Lang.Types;

namespace Fl.TypeChecking.Inferrers
{
    class VariableTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstVariableNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor checker, AstVariableNode vardecl)
        {
            switch (vardecl)
            {
                case AstVarDefinitionNode vardefnode:
                    return VarDefinitionNode(checker, vardefnode);

                case AstVarDestructuringNode vardestnode:
                    return VarDestructuringNode(checker, vardestnode);
            }
            throw new AstWalkerException($"Invalid variable declaration of type {vardecl.GetType().FullName}");
        }

        protected InferredType VarDefinitionNode(TypeInferrerVisitor checker, AstVarDefinitionNode vardecl)
        {
            // Get the variable type from the declaration
            InferredType inferredType = null;

            foreach (var declaration in vardecl.VarDefinitions)
            {
                var lhs = checker.SymbolTable.GetSymbol(declaration.Item1.Value.ToString());

                if (lhs.Type == null)
                    checker.Constraints.AssignTemporalType(lhs);

                if (inferredType == null)
                    inferredType = new InferredType(lhs.Type);

                // If it is a variable definition, get the right-hand side type info
                var rhsType = declaration.Item2?.Visit(checker);

                // When lhs is "var", take the type from the right hand side expression, or throw if it is not available
                if (inferredType.Type is Anonymous)
                {
                    checker.Constraints.InferTypeAs(inferredType.Type as Anonymous, rhsType.Type);
                    inferredType = new InferredType(rhsType?.Type ?? throw new SymbolException("Implicitly-typed variable must be initialized"), declaration.Item1.Value.ToString());
                }
                else if (rhsType.Type is Anonymous)
                {
                    checker.Constraints.InferTypeAs(rhsType.Type as Anonymous, inferredType.Type);
                }
                else if (inferredType.Type != null && !inferredType.Type.IsAssignableFrom(rhsType?.Type))
                {
                    throw new SymbolException($"Cannot assign type {rhsType.Type} to variable of type {inferredType.Type}");
                }
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
