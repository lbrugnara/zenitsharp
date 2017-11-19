// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Symbols
{
    public class Symbol
    {
        protected ObjectType _ObjectType;
        protected StorageType _StorageType;
        protected FlObject _Value;
        protected string _Name;

        public Symbol(ObjectType type, StorageType storage)
        {
            _ObjectType = type;
            _StorageType = storage;
        }

        public ObjectType Type => _ObjectType;
        public StorageType Storage => _StorageType;
        public FlObject Binding => _Value;
        public string Name => _Name;

        public override string ToString()
        {
            return _Name ?? "";
        }

        public void DoBinding(string scope, string name, FlObject val)
        {
            _Name = $"{scope}.{name}";
            _ObjectType = val.Type;
            _Value = val;
            _Value.RefInc(this);
        }

        public void UpdateBinding(FlObject newval)
        {
            _ObjectType = newval.Type;
            _Value.RefDec(this);
            _Value = newval;
            _Value.RefInc(this);
        }
    }
}
