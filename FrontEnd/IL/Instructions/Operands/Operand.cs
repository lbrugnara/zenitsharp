// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Zenit.Engine.Symbols.Types;

namespace Zenit.IL.Instructions.Operands
{
    public abstract class Operand
    {
        public OperandType Type { get; }
        public SymbolOperand Member { get; private set; }

        public Operand(OperandType type)
        {
            this.Type = type;
        }

        public void AddMember(SymbolOperand member)
        {
            if (this.Member == null)
                this.Member = member;
            else
                this.Member.AddMember(member);
        }
    }
}
