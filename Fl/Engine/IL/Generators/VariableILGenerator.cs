// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions;
using Fl.Engine.IL.Instructions.Operands;
using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;
using Fl.Parser;
using Fl.Parser.Ast;
using System;
using System.Linq;

namespace Fl.Engine.IL.Generators
{
    class VariableILGenerator : INodeVisitor<AstILGenerator, AstVariableNode, Operand>
    {
        public Operand Visit(AstILGenerator generator, AstVariableNode vardecl)
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

        protected Operand VarDefinitionNode(AstILGenerator generator, AstVarDefinitionNode vardecl)
        {
            // Get the variable type
            ObjectType t = ObjectType.GetFromTokenType(vardecl.VarType.TypeToken.Type) ?? ObjectType.GetFromTypeName(vardecl.VarType.TypeToken.Value.ToString());

            foreach (var declaration in vardecl.VarDefinitions)
            {
                // Get the identifier name
                var identifierToken = declaration.Item1;
                // Get the right-hand side operand
                var operand = declaration.Item2?.Exec(generator);

                // var <identifier> = <operand>
                var symbol = new SymbolOperand(identifierToken.Value.ToString());
                generator.Emmit(new VarInstruction(symbol, t?.ClassName ?? operand?.TypeName, operand));
            }
            return null;
        }

        protected Operand VarDestructuringNode(AstILGenerator generator, AstVarDestructuringNode vardestnode)
        {
            // Get the variable type
            ObjectType dataType = ObjectType.GetFromTokenType(vardestnode.VarType.TypeToken.Type);

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
