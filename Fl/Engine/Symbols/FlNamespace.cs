// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Symbols
{
    public class FlNamespace : FlObject
    {
        private FlNamespace _Parent;
        private Dictionary<string, Symbol> _Map;

        public FlNamespace(string name, FlNamespace parent = null)
            : base(ObjectType.Namespace, null)
        {
            _ObjectType = ObjectType.Namespace;
            _Value = this;
            _Map = new Dictionary<string, Symbol>();
            Name = name;
            _Parent = parent;            
            if (_Parent != null)
            {
                _Parent.AddSymbol(Name,new Symbol(ObjectType.Namespace, StorageType.Constant), this);
            }
        }

        public string Name  { get; }

        public string FullName => (_Parent != null ? $"{_Parent.FullName}." : "") + $"{Name}";

        #region Indexers
        public Symbol this[string var]
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
            s.DoBinding($"{Owner?.Name ?? Name}", name, o);
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

        public string ShowNamespace(int tabs = 0)
        {
            string str = new string(' ', tabs) + base.ToDebugStr();
            int childindent = tabs + 2;
            string indent = new string(' ', childindent);
            foreach (var key in _Map.Keys)
            {
                Symbol child = _Map[key];
                if (child.Type == ObjectType.Namespace)
                    str += $"\n{child.Binding.AsNamespace.ShowNamespace(childindent)}";
                else
                    str += $"\n{indent}{child.Binding.ToDebugStr()}";
            }
            return str;
        }
    }
}
