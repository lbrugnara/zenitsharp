// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Symbols.Objects
{
    public class FlNamespace : FlObject
    {
        private FlNamespace _Parent;
        private Dictionary<string, Symbol> _Map;
        private string _Name;

        public override object RawValue => FullName; // TODO:

        public override bool IsPrimitive => false;

        public override ObjectType ObjectType => NamespaceType.Value;

        public FlNamespace(string name, FlNamespace parent = null)
        {
            _Name = name;
            _Map = new Dictionary<string, Symbol>();            
            _Parent = parent;            
            if (_Parent != null)
            {
                _Parent.AddSymbol(_Name, new Symbol(StorageType.Constant), this);
            }
        }

        public string Name => _Name;

        public string FullName => (_Parent != null ? $"{_Parent.FullName}." : "") + $"{_Name}";

        #region Indexers
        public override Symbol this[string var]
        {
            get
            {
                if (_Map.ContainsKey(var))
                    return _Map[var];
                return null;
            }
        }

        public void AddSymbol(string name, Symbol s, FlObject o)
        {
            s.DoBinding(_Name, name, o);
            _Map[name] = s;
        }
        #endregion

        public Dictionary<string, Symbol> Members
        {
            get
            {
                return _Map;
            }
        }

        public override FlObject Clone()
        {
            return new FlNamespace(_Name, _Parent);
        }

        public override FlObject ConvertTo(ObjectType type)
        {
            throw new CastException($"Cannot convert type {ObjectType} to {type}");
        }
    }
}
