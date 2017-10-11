// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine
{
    public enum ScopeType
    {
        Common,
        Function,
        Loop
    }

    public class Scope
    {
        #region Private Constants
        private const string FlReturnKey = "@__flreturn";
        #endregion

        #region Private Fields
        private Scope _Parent;
        private ScopeType _ScopeType;
        private Dictionary<string, ScopeEntry> _Map;

        private bool _Break;
        private bool _Continue;
        #endregion

        #region Constructors
        public Scope(ScopeType type, Scope parent = null)
        {
            _Parent = parent;
            _ScopeType = type;
            _Map = new Dictionary<string, ScopeEntry>();
        }

        public Scope(Scope parent = null)
        {
            _Parent = parent;
            _ScopeType = ScopeType.Common;
            _Map = new Dictionary<string, ScopeEntry>();
        }
        #endregion

        #region Indexers
        public ScopeEntry this[string var]
        {
            get
            {
                var scp = this;
                while (scp != null)
                {
                    if (scp._Map.ContainsKey(var))
                        return scp._Map[var];
                    scp = scp._Parent;
                }
                return null;
            }
        }

        public ScopeEntry this[object var]
        {
            get
            {
                return var == null ? null : this[var.ToString()];
            }
        }
       /* public ScopeEntry this[string var]
        {
            get
            {
                var scp = this;
                while (scp != null)
                {
                    if (scp._Map.ContainsKey(var))
                        return scp._Map[var];
                    scp = scp._Parent;
                }
                return null;
            }
            set
            {
                if (!_Map.ContainsKey(var) && (_Parent == null || _Parent.IsDefined(var, true)))
                    throw new AstWalkerException($"Symbol {var} does not exist in the current context");
                if (_Map.ContainsKey(var))
                    _Map[var] = value;
                else
                    _Parent[var] = value;
            }
        }

        public ScopeEntry this[object var]
        {
            get
            {
                return var == null ? null : this[var.ToString()];
            }
            set
            {
                if (var != null)
                    this[var.ToString()] = value;
            }
        }*/
            #endregion

            #region Public Properties
        public ScopeType ScopeType { get => _ScopeType; }

        public bool MustReturn
        {
            get
            {
                var scp = this;
                while (scp != null)
                {
                    if (scp._ScopeType == ScopeType.Function && scp._Map.ContainsKey(FlReturnKey))
                        return true;
                    scp = scp._Parent;
                }
                return false;
            }
        }

        public bool MustBreak
        {
            get
            {
                var scp = this;
                while (scp != null)
                {
                    if (scp._ScopeType == ScopeType.Loop && scp._Break)
                        return true;
                    scp = scp._Parent;
                }
                return false;
            }
        }

        public bool MustContinue
        {
            get
            {
                var scp = this;
                while (scp != null)
                {
                    if (scp._ScopeType == ScopeType.Loop && scp._Continue)
                        return true;
                    scp = scp._Parent;
                }
                return false;
            }
        }

        public ScopeEntry ReturnValue
        {
            get
            {
                return _Map.ContainsKey(FlReturnKey) ? _Map[FlReturnKey] : null;
            }
            set
            {
                var scp = this;
                while (scp != null)
                {
                    if (scp._ScopeType == ScopeType.Function)
                    {
                        scp._Map[FlReturnKey] = value;
                        return;
                    }
                    scp = scp._Parent;
                }
                throw new AstWalkerException("Cannot return a value from a non-function scope");
            }
        }

        #endregion

        #region Public Methods
        public void NewSymbol(string name, ScopeEntry initializer = null)
        {
            if (_Map.ContainsKey(name))
                throw new AstWalkerException($"Symbol {name} is already defined in this scope");

            //if (_ScopeType != ScopeType.Function && _Parent != null && _Parent.IsDefined(name, true))
            //    throw new AstWalkerException($"Symbol {name} is already defined in an enclosing scope");

            _Map[name] = initializer;
        }

        public void UpdateSymbol(string name, ScopeEntry value)
        {
            if (!_Map.ContainsKey(name) && (_Parent == null || _Parent.IsDefined(name, true)))
                throw new AstWalkerException($"Symbol {name} does not exist in the current context");

            if (_Map.ContainsKey(name))
            {
                _Map[name] = value;
            }
            else
            {
                _Parent.UpdateSymbol(name, value);
            }
        }

        public bool IsDefined(string var, bool inChain = false)
        {
            if (inChain)
            {
                var scp = this;
                while (scp != null)
                {
                    if (scp._Map.ContainsKey(var))
                        return true;
                    scp = scp._Parent;
                }
                return false;
            }
            return _Map.ContainsKey(var);
        }

        public void SetBreak(int nbreaks)
        {
            if (_ScopeType != ScopeType.Loop && _Parent == null)
                throw new AstWalkerException("Cannot break in a non-loop scope");

            var scp = this;
            while (scp != null)
            {
                if (scp._ScopeType == ScopeType.Loop)
                {
                    scp._Break = true;
                    if (--nbreaks == 0)
                        break;
                }
                scp = scp._Parent;
                if (scp == null && nbreaks >= 0)
                    throw new AstWalkerException(nbreaks > 0 ? $"Cannot break more than {nbreaks} loops" : "Cannot break in a non-loop scope");
            }
        }

        public void SetContinue()
        {
            if (_ScopeType != ScopeType.Loop && _Parent == null)
                throw new AstWalkerException("Cannot continue in a non-loop scope");

            var scp = this;
            while (scp != null)
            {
                if (scp._ScopeType == ScopeType.Loop)
                {
                    scp._Continue = true;
                    break;
                }
                scp = scp._Parent;
                if (scp == null)
                    throw new AstWalkerException("Cannot continue in a non-loop scope");
            }
        }

        public void DoContinue()
        {
            if (_ScopeType != ScopeType.Loop)
                throw new AstWalkerException("Cannot continue in a non-loop scope");
            _Continue = false;
        }

        #endregion

        #region Private Methods
        private bool IsScopeType(ScopeType type)
        {
            var scp = this;
            while (scp != null)
            {
                if (scp._ScopeType == type)
                    return true;
                scp = scp._Parent;
            }
            return false;
        }
        #endregion
    }
}
