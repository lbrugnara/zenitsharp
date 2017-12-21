// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Types;
using System.Collections.Generic;

namespace Fl.Engine.Symbols.Objects
{
    public class FlInt : FlInstance
    {
        #region Private Fields
        private int _RawValue;
        #endregion

        #region Constructor
        public FlInt(int value)
        {
            _RawValue = value;
        }
        #endregion

        #region Public Properties
        public int Value { get => _RawValue; set => _RawValue = value; }
        #endregion

        #region FlObject implementation

        public override FlType Type => FlIntType.Instance;

        public override object RawValue => _RawValue;

        public override FlObject Clone()
        {
            return new FlInt(_RawValue);
        }

        #endregion
    }
}
