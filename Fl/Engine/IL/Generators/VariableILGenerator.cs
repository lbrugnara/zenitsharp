// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions;
using Fl.Engine.IL.Instructions.Operands;
using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Types;
using Fl.Parser.Ast;

namespace Fl.Engine.IL.Generators
{
    class VariableILGenerator : INodeVisitor<ILGenerator, AstVariableNode, Operand>
    {
        public Operand Visit(ILGenerator generator, AstVariableNode vardecl)
        {
            switch (vardecl)
            {
                case AstVarDefinitionNode vardefnode:
                    return VarDefinitionNode(generator, vardefnode);

                case AstVarDestructuringNode vardestnode:
                    return VarDestructuringNode(generator, vardestnode);
            }
            throw new AstWalkerException($"Invalid variable declaration of type {vardecl.GetType().FullName}");
        }

        protected Operand VarDefinitionNode(ILGenerator generator, AstVarDefinitionNode vardecl)
        {
            // Get the variable type
            TypeResolver typeresolver = TypeResolver.GetTypeResolverFromToken(vardecl.VarType.TypeToken);

            foreach (var declaration in vardecl.VarDefinitions)
            {
                // Get the identifier name
                var identifierToken = declaration.Item1;

                if (generator.SymbolTable.SymbolIsDefinedInBlock(identifierToken.Value.ToString()))
                    throw new SymbolException($"Symbol {identifierToken.Value} is already defined.");

                // Get the right-hand side operand
                var operand = declaration.Item2?.Exec(generator);

                if (typeresolver.TypeName == FlNullType.Instance.Name && operand != null && operand.TypeResolver.TypeName != FlNullType.Instance.Name)
                    typeresolver = operand.TypeResolver;

                // var <identifier> = <operand>
                var symbol = generator.SymbolTable.NewSymbol(identifierToken.Value.ToString(), typeresolver);
                generator.Emmit(new VarInstruction(symbol, typeresolver, operand));
            }
            return null;
        }

        protected Operand VarDestructuringNode(ILGenerator generator, AstVarDestructuringNode vardestnode)
        {
            // Get the variable type
            TypeResolver typeresolver = TypeResolver.GetTypeResolverFromToken(vardestnode.VarType.TypeToken);

            /*vardestnode.DestructInit.Exec()

            foreach (var declaration in vardestnode.VarDefinitions)
            {
                var identifierToken = declaration.Item1;
                var initializerInstr = declaration.Item2?.Exec(generator);

                generator.Emmit(new LocalVarInstruction(dataType, identifierToken.Value.ToString(), initializerInstr?.TargetName));
            }*/
            return null;
        }
    }
}
