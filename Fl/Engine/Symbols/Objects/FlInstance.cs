// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;
using System.Collections.Generic;
using System.Text;
using Fl.Engine.Symbols.Types;
using Fl.Engine.Symbols.Exceptions;

namespace Fl.Engine.Symbols.Objects
{
    public class FlInstance : FlObject
    {
        private FlClass _Class;

        public FlInstance(FlClass clasz)
        {
            _Class = clasz;
        }

        #region FlObject implementation

        public override object RawValue => _Class;

        public override bool IsPrimitive => false;

        public override ObjectType ObjectType => _Class.ObjectType;

        public override FlObject Clone()
        {
            return new FlInstance(_Class.Clone() as FlClass);
        }

        public override FlObject ConvertTo(ObjectType type)
        {
            return _Class.ConvertTo(type);
        }

        public override string ToString()
        {
            return $"instance {_Class}";
        }
        #endregion

        #region Public Properties

        public FlObject this[MemberType type, string name]
        {
            get
            {
                Symbol s = _Class[type, name];
                return s.Binding;
            }
            set
            {
                Symbol s = _Class[type, name];
                s.UpdateBinding(value);
            }
        }

        public override Symbol this[string name]
        {
            get
            {
                return _Class[name];
            }
        }

        #endregion
    }
}
