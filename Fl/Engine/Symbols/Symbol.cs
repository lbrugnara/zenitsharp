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

        public Symbol(ObjectType type, StorageType storage, FlObject value = null)
        {
            _ObjectType = type;
            _StorageType = storage;
            _Value = value;
            _Value.RefInc(this);
        }

        public ObjectType Type => _ObjectType;
        public StorageType Storage => _StorageType;
        public FlObject Binding => _Value;
        public string Name { get => _Name; set => _Name = value; }

        public void UpdateBinding(FlObject newval)
        {
            _ObjectType = newval.Type;
            _Value.RefDec(this);
            _Value = newval;
            _Value.RefInc(this);
        }
    }
}
