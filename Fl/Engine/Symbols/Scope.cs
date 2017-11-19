// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Symbols
{
    public enum ScopeType
    {
        Common,
        Function,
        Loop
    }

    public class Scope
    {
        #region Static fields
        private static int ScopeN = 1;
        #endregion

        #region Constants
        public const string FlReturnKey = "@flreturn";
        #endregion

        #region Private Fields
        private ScopeType _ScopeType;
        private Dictionary<string, Symbol> _Map;
        // Contains the environment of the binding time for closures
        private Scope _Env;
        private string _Name;
        private bool _Break;
        private bool _Continue;
        #endregion

        #region Constructors
        public Scope(ScopeType type, Scope env = null)
        {
            _ScopeType = type;
            _Map = new Dictionary<string, Symbol>();
            _Env = env;
            _Name = ScopeN == 1 ? "<global>" : $"<scope@{{{ScopeN++}}}>";
        }
        #endregion

        #region Indexers
        public Symbol this[string var]
        {
            get
            {
                return _Map.ContainsKey(var) ? _Map[var] : _Env != null && _Env._Map.ContainsKey(var) ? _Env._Map[var] : null;
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
        public bool HasSymbol(string s) => _Map.ContainsKey(s) || _Env != null && _Env._Map.ContainsKey(s);

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
            else if (_Env != null && _Env._Map.ContainsKey(name))
            {
                _Env._Map[name].UpdateBinding(obj);
            }
            else throw new Parser.Ast.AstWalkerException($"Symbol {name} does not exist in the current context");
        }

        public void Import(Scope scope)
        {
            var keys = scope._Map.Keys;
            foreach (var k in keys)
            {
                var s = scope._Map[k];
                if (s.Binding.IsCallable && s.Storage == StorageType.Constant)
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
