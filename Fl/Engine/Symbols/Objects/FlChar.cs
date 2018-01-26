// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Types;

namespace Fl.Engine.Symbols.Objects
{
    public class FlChar : FlInstance
    {
        #region Private Fields
        private char _RawValue;
        #endregion

        #region Constructor
        public FlChar(char value)
        {
            _RawValue = value;
        }
        #endregion

        #region Public Properties
        public char Value { get => _RawValue; set => _RawValue = value; }
        #endregion

        #region FlObject implementation

        public override FlType Type => FlCharType.Instance;

        public override object RawValue => _RawValue;

        public override FlObject Clone()
        {
            return new FlChar(_RawValue);
        }

        #endregion
    }
}
