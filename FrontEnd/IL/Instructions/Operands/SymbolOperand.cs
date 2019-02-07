// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Zenit.Engine.Symbols.Types;
using System;

namespace Zenit.IL.Instructions.Operands
{
    public class SymbolOperand : Operand
    {
        public string Name { get; }
        public string Scope { get; private set; }
        public string MangledName => Scope != null ? $"{Name}__{{{Scope}}}" : Name;
        public bool IsResolved { get; private set; }

        public SymbolOperand(string name, OperandType type, string scope, bool isResolved = true)
            : base (type)
        {
            this.Name = name;
            this.Scope = scope ?? throw new ArgumentNullException(nameof(scope));
            this.IsResolved = isResolved;
        }

        public SymbolOperand(string name, OperandType type)
            : base(type)
        {
            this.Name = name;
        }

        public SymbolOperand(string name)
            : base(OperandType.Auto)
        {
            this.Name = name;
        }

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
