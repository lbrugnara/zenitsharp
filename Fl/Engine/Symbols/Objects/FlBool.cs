// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Types;

namespace Fl.Engine.Symbols.Objects
{
    public class FlBool : FlInstance
    {
        private bool rawValue;

        public FlBool(bool value)
        {
            this.rawValue = value;
        }

        public override FlType Type => FlBoolType.Instance;

        public override object RawValue => this.rawValue;

        public bool Value { get => this.rawValue; set => this.rawValue = value; }

        public override FlObject Clone()
        {
            return new FlBool(this.rawValue);
        }
    }
}
