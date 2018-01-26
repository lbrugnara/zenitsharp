// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Engine.Symbols.Objects
{
    public class FlInstance : FlObject
    {
        private FlType _Type;

        protected FlInstance()
        {
        }

        public FlInstance(FlType type)
        {
            _Type = type;
        }

        #region FlObject implementation

        public override object RawValue => Type;

        public override FlType Type => _Type.Type;

        public override FlObject Clone()
        {
            return new FlInstance(Type.Clone() as FlType);
        }

        public override string ToString()
        {
            return $"{RawValue} ({Type.ToString()})";
        }

        public override string ToDebugStr()
        {
            return $"{RawValue} ({Type.ToDebugStr()})";
        }

        #endregion
    }
}
