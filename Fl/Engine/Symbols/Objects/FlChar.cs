// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Types;

namespace Fl.Engine.Symbols.Objects
{
    public class FlChar : FlInstance
    {
        #region Private Fields
        private char rawValue;
        #endregion

        #region Constructor
        public FlChar(char value)
        {
            this.rawValue = value;
        }
        #endregion

        #region Public Properties
        public char Value { get => this.rawValue; set => this.rawValue = value; }
        #endregion

        #region FlObject implementation

        public override FlType Type => FlCharType.Instance;

        public override object RawValue => this.rawValue;

        public override FlObject Clone()
        {
            return new FlChar(this.rawValue);
        }

        #endregion
    }
}
