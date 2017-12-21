// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Engine.Symbols.Types;

namespace Fl.Engine.IL.Instructions.Operands
{
    public class SymbolOperand : Operand
    {
        public string Name { get; }

        public SymbolOperand(string name, TypeResolver type = null)
            : base (type)
        {
            this.Name = name;
        }

        public override string ToString()
        {
            string s = this.Name;
            if (this.Member != null)
                s += $".{this.Member}";
            return s;
        }
    }
}
