// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.IL.Instructions.Operands;
using Fl.Engine.Symbols.Types;
using Fl.Ast;

namespace Fl.IL.Generators
{
    class AccessorILGenerator : INodeVisitor<ILGenerator, AstAccessorNode, Operand>
    {
        public Operand Visit(ILGenerator generator, AstAccessorNode accessor)
        {
            string currentIdentifier = accessor.Identifier.Value.ToString();

            // If current ID is the root member (or the only accessed one) return its symbol
            if (accessor.Enclosing == null)
                // If the symbol is not defined, create a new one that is not resolved yet (it could be a symbol that is not yet defined)
                return generator.SymbolTable.GetSymbol(currentIdentifier) ?? new SymbolOperand(currentIdentifier, OperandType.Auto, generator.SymbolTable.CurrentBlock.Name, false);

            // If not, resolve its parrent...
            Operand parent = accessor.Enclosing.Visit(generator);

            // ... add current ID as accessor's member of parent
            parent.AddMember(new SymbolOperand(currentIdentifier));

            return parent;
        }
    }
}
