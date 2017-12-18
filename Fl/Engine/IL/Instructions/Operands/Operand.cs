// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Engine.IL.Instructions.Operands
{
    public abstract class Operand
    {
        public string TypeName { get; }

        public Operand(string type)
        {
            this.TypeName = type;
        }
    }
}
