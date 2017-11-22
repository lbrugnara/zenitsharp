// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Types;
using System.Collections.Generic;

namespace Fl.Engine.Symbols.Objects
{
    public class FlInteger : FlObject
    {
        #region Private Fields
        private int _RawValue;
        private Dictionary<string, Symbol> _Members;
        #endregion

        #region Constructor
        public FlInteger(int value)
        {
            _RawValue = value;
            _Members = new Dictionary<string, Symbol>();
            RegisterMembers();
        }
        #endregion

        #region Public Properties
        public int Value { get => _RawValue; set => _RawValue = value; }
        #endregion

        #region FlObject implementation
        public override ObjectType ObjectType => IntegerType.Value;

        public override object RawValue => _RawValue;

        public override bool IsPrimitive => true;

        public override FlObject Clone()
        {
            return new FlInteger(_RawValue);
        }

        public override FlObject ConvertTo(ObjectType type)
        {
            if (type == IntegerType.Value)
            {
                return this.Clone();
            }
            else if (type == DoubleType.Value)
            {
                return new FlDouble((double)_RawValue);
            }
            else if (type == DecimalType.Value)
            {
                return new FlDecimal((decimal)_RawValue);
            }
            else if (type == StringType.Value)
            {
                return new FlString(_RawValue.ToString());
            }
            throw new CastException($"Cannot convert type {ObjectType} to {type}");
        }

        public override Symbol this[string membername]
        {
            get
            {
                if (_Members.ContainsKey(membername))
                    return _Members[membername];
                return base[membername];
            }
        }
        #endregion

        #region Private methods
        public void RegisterMembers()
        {
            Symbol ts = new Symbol(StorageType.Constant);
            ts.DoBinding("int", "str", new FlMethod("str", this, (self, args) => new FlString(self.RawValue.ToString())));
            _Members["str"] = ts;
        }        
        #endregion
    }
}
