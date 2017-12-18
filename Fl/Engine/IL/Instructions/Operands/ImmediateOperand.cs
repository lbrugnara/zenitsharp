// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Engine.IL.Instructions.Operands
{
    public class ImmediateOperand : Operand
    {                
        public object Value { get; }

        public ImmediateOperand(string type, object val)
            : base(type)
        {            
            this.Value = val;
        }

        public override string ToString()
        {
            return this.Value?.ToString();
        }
    }
}
