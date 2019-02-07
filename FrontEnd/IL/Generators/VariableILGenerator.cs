// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.IL.Instructions;
using Zenit.IL.Instructions.Operands;
using Zenit.Engine.Symbols.Types;
using Zenit.Ast;
using Zenit.Semantics.Exceptions;

namespace Zenit.IL.Generators
{
    class VariableILGenerator : INodeVisitor<ILGenerator, VariableNode, Operand>
    {
        public Operand Visit(ILGenerator generator, VariableNode vardecl)
        {
            switch (vardecl)
            {
                case VariableDefinitionNode vardefnode:
                    return VarDefinitionNode(generator, vardefnode);

                case VariableDestructuringNode vardestnode:
                    return VarDestructuringNode(generator, vardestnode);
            }
            throw new AstWalkerException($"Invalid variable declaration of type {vardecl.GetType().FullName}");
        }

        protected Operand VarDefinitionNode(ILGenerator generator, VariableDefinitionNode vardecl)
        {
            foreach (var definition in vardecl.Definitions)
            {
                // Get the identifier name
                var variableName = definition.Left.Value;

                // Check if the symbol is already defined
                if (generator.SymbolTable.SymbolIsDefinedInBlock(variableName))
                    throw new SymbolException($"Symbol {variableName} is already defined.");

                // If it is a variable definition, emmit the code to resolve the right-hand side
                var rhs = definition.Right?.Visit(generator);

                // Get the variable type from the declaration
                var type = OperandType.FromToken(vardecl.Information.Type);

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

        protected Operand VarDestructuringNode(ILGenerator generator, VariableDestructuringNode vardestnode)
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
