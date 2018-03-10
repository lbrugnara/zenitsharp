// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Types;

namespace Fl.Engine.Symbols.Objects
{
    public class FlFloat : FlInstance
    {
        private float rawValue;

        public FlFloat(float value)
        {
            this.rawValue = value;
        }

        public override FlType Type => FlFloatType.Instance;

        public override object RawValue => this.rawValue;

        public float Value { get => this.rawValue; set => this.rawValue = value; }

        public override FlObject Clone()
        {
            return new FlFloat(this.rawValue);
        }
    }
}
