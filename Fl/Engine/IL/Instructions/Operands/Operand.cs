// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Engine.Symbols.Types;

namespace Fl.Engine.IL.Instructions.Operands
{
    public abstract class Operand
    {
        public TypeResolver TypeResolver { get; }
        public SymbolOperand Member { get; private set; }

        public Operand(TypeResolver type)
        {
            this.TypeResolver = type;
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
