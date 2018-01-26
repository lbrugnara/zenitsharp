// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;
using System.Collections.Generic;

namespace Fl.Engine.Symbols
{
    public class Scope
    {
        #region Constants
        public const string FlReturnKey = "@flreturn";
        #endregion

        #region Private Fields
        private ScopeType _ScopeType;
        private Dictionary<string, Symbol> _Map;
        // Contains the environment of the binding time for closures
        private ScopeChain _Env;
        private string _Name;
        private bool _Break;
        private bool _Continue;
        #endregion

        #region Constructors
        public Scope(ScopeType type, string label, ScopeChain env = null)
        {
            _ScopeType = type;
            _Map = new Dictionary<string, Symbol>();
            _Env = env;
            _Name = label;
        }
        #endregion

        public string Name => _Name;

        #region Indexers
        public Symbol this[string var]
        {
            get
            {
                return _Map.ContainsKey(var) ? _Map[var] : _Env != null && _Env.HasSymbol(var) ? _Env.GetSymbol(var) : null;
            }
        }

        public Symbol this[object var]
        {
            get
            {
                return var == null ? null : this[var.ToString()];
            }
        }
        #endregion

        #region Public Properties
        public bool HasSymbol(string s) => _Map.ContainsKey(s) || _Env != null && _Env.HasSymbol(s);

        public bool Break { get => _Break; set => _Break = value; }

        public bool Continue { get => _Continue; set => _Continue = value; }

        public ScopeType ScopeType { get => _ScopeType; }
        #endregion

        #region Public Methods
        public void AddSymbol(string name, Symbol symbol, FlObject binding)
        {
            symbol.DoBinding(_Name, name, binding);
            _Map[name] = symbol;
        }

        public void UpdateSymbol(string name, FlObject obj)
        {
            if (_Map.ContainsKey(name))
            {
                _Map[name].UpdateBinding(obj);
            }
            else if (_Env != null && _Env.HasSymbol(name))
            {
                _Env.GetSymbol(name).UpdateBinding(obj);
            }
            else throw new SymbolException($"Symbol {name} does not exist in the current context");
        }

        public void Import(Scope scope)
        {
            var map = new Dictionary<string, Symbol>(scope._Map);
            var keys = map.Keys;
            foreach (var k in keys)
            {
                var s = map[k];
                if (s.Binding.Type == FlFuncType.Instance && s.SymbolType == SymbolType.Constant)
                    continue;
                _Map[k] = s;
            }
        }

        public void Using(FlNamespace ns)
        {
            var keys = ns.Members.Keys;
            foreach (var k in keys)
            {
                _Map[k] = ns.Members[k];
            }
        }
        #endregion
    }
}
