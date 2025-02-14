﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Zenit.Engine.Symbols.Types;

namespace Zenit.IL.Instructions.Operands
{
    public class ImmediateOperand : Operand
    {                
        public object Value { get; }

        public ImmediateOperand(OperandType type, object val)
            : base(type)
        {            
            this.Value = val;
        }

        public override string ToString()
        {
            string s = this.Value?.ToString();
            if (this.Member != null)
                s += $".{this.Member}";
            return s;
        }
    }
}
