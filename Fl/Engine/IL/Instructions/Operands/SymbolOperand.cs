// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Engine.Symbols.Types;

namespace Fl.Engine.IL.Instructions.Operands
{
    public class SymbolOperand : Operand
    {
        public string Name { get; }
        public string Scope { get; private set; }
        public string MangledName => Scope != null ? $"{Name}__`{Scope}" : Name;

        public SymbolOperand(string name, string scope, TypeResolver type = null)
            : base (type)
        {
            this.Name = name;
            this.Scope = scope;
        }

        public bool IsResolved => Scope != null;

        public void SetScope(string scope)
        {
            if (Scope != null)
                throw new System.Exception("Cannot re-bind variable");
            Scope = scope;
        }

        public override string ToString()
        {
            string s = this.MangledName;
            if (this.Member != null)
                s += $".{this.Member}";
            return s;
        }
    }
}
