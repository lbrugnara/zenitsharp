// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.IL.Instructions;
using Fl.IL.Instructions.Operands;
using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Types;
using Fl.Parser.Ast;
using Fl.Symbols;

namespace Fl.IL.Generators
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
            foreach (var declaration in vardecl.VarDefinitions)
            {
                // Get the identifier name
                var variableName = declaration.Item1.Value.ToString();

                // Check if the symbol is already defined
                if (generator.SymbolTable.SymbolIsDefinedInBlock(variableName))
                    throw new SymbolException($"Symbol {variableName} is already defined.");

                // If it is a variable definition, emmit the code to resolve the right-hand side
                var rhs = declaration.Item2?.Visit(generator);

                // Get the variable type from the declaration
                var type = OperandType.FromToken(vardecl.VarType.TypeToken);

                if (type == OperandType.Auto)
                {
                    // If variable is implicitly typed, the right-hand side expression is a must
                    if (rhs == null)
                        throw new AstWalkerException($"Implicitly-typed variable must be initialized");

                    // Take the type from the right hand side expression
                    type = rhs.Type;
                }

                // Create the new symbol for the variable
                var symbol = generator.SymbolTable.NewSymbol(variableName, type);

                generator.Emmit(new VarInstruction(symbol, type, rhs));
            }
            return null;
        }

        protected Operand VarDestructuringNode(ILGenerator generator, AstVarDestructuringNode vardestnode)
        {
            // Get the variable type
            //TypeResolver typeresolver = TypeResolver.GetTypeResolverFromToken(vardestnode.VarType.TypeToken);

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
