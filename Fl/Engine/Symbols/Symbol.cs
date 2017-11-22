// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Symbols
{
    public class Symbol
    {
        protected StorageType _StorageType;
        protected FlObject _Binding;
        protected string _Name;

        public Symbol(StorageType storage)
        {
            _StorageType = storage;
            _Binding = FlNull.Value;
        }

        public ObjectType ObjectType => _Binding?.ObjectType;
        public StorageType Storage => _StorageType;
        public FlObject Binding => _Binding;
        public string Name => _Name;

        public override string ToString()
        {
            return _Name ?? "";
        }

        public void DoBinding(string scope, string name, FlObject val)
        {
            if (_Binding != FlNull.Value)
                throw new SymbolException($"Cannot re-bind symbol {Name}");
            _Name = $"{scope}.{name}";
            if (val == null)
                val = FlNull.Value;
            _Binding = val;
        }

        public void UpdateBinding(FlObject newval)
        {
            if (_Binding != FlNull.Value && _Binding.ObjectType != newval.ObjectType)
                throw new SymbolException($"Cannot implicitly convert type '{newval.ObjectType}' to '{_Binding.ObjectType}'");
            _Binding = newval;
        }
    }
}
