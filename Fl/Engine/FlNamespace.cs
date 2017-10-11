// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine
{
    public class FlNamespace : ScopeEntry
    {
        private FlNamespace _Parent;
        private Dictionary<string, ScopeEntry> _Map;

        public FlNamespace(string name, FlNamespace parent = null)
        {
            _DataType = ScopeEntryType.Namespace;
            _Value = this;
            _Map = new Dictionary<string, ScopeEntry>();
            Name = name;
            _Parent = parent;            
            if (_Parent != null)
            {
                _Parent[Name] = this;
            }
        }

        public string Name  { get; }

        public string FullName => (_Parent != null ? $"{_Parent.FullName}." : "") + $"{Name}";

        #region Indexers
        public ScopeEntry this[string var]
        {
            get
            {
                if (_Map.ContainsKey(var))
                    return _Map[var];
                return null;
            }
            set
            {
                _Map[var] = value;
            }
        }
        #endregion

        public Dictionary<string, ScopeEntry> Members
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
                ScopeEntry child = _Map[key];
                if (child.IsNamespace)
                    str += $"\n{child.NamespaceValue.ShowNamespace(childindent)}";
                else
                    str += $"\n{indent}{child.ToDebugStr()}";
            }
            return str;
        }
    }
}
