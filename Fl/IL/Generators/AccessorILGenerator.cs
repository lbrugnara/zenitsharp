// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.IL.Instructions.Operands;
using Fl.Engine.Symbols.Types;
using Fl.Ast;

namespace Fl.IL.Generators
{
    class AccessorILGenerator : INodeVisitor<ILGenerator, AccessorNode, Operand>
    {
        public Operand Visit(ILGenerator generator, AccessorNode accessor)
        {
            string currentIdentifier = accessor.Target.Value;

            // If current ID is the root member (or the only accessed one) return its symbol
            if (accessor.Parent == null)
                // If the symbol is not defined, create a new one that is not resolved yet (it could be a symbol that is not yet defined)
                return generator.SymbolTable.GetSymbol(currentIdentifier) ?? new SymbolOperand(currentIdentifier, OperandType.Auto, generator.SymbolTable.CurrentBlock.Name, false);

            // If not, resolve its parent...
            Operand parent = accessor.Parent.Visit(generator);

            // ... add current ID as accessor's member of parent
            parent.AddMember(new SymbolOperand(currentIdentifier));

            return parent;
        }
    }
}
