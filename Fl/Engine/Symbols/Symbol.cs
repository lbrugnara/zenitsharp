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
        protected SymbolType _SymbolType;
        protected FlObject _Binding;
        protected string _Name;
        protected string _ScopeName;

        public Symbol(SymbolType type, StorageType storage = StorageType.Auto)
        {
            _SymbolType = type;
            _StorageType = storage;
            _Binding = FlNull.Value;
        }

        public ObjectType ObjectType => _Binding?.ObjectType;
        public SymbolType SymbolType => _SymbolType;
        public StorageType StorageType => _StorageType;
        public FlObject Binding => _Binding;
        public string Name => _Name;

        public string FullName => $"{_ScopeName}.{_Name}";

        public override string ToString()
        {
            string s = _Name;
            if (_Binding != null) s += $" ({_Binding})";
            return s;
        }

        public Symbol Clone(bool doBinding)
        {
            Symbol s = new Symbol(_SymbolType, _StorageType);
            if (doBinding && _Binding != null)
                s.DoBinding(_ScopeName, _Name, _Binding.Clone());
            return s;
        }

        public void DoBinding(string scope, string name, FlObject val)
        {
            if (_Binding != FlNull.Value)
                throw new SymbolException($"Cannot re-bind symbol {Name}");
            _ScopeName = scope;
            _Name = name;
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
