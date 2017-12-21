// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Objects;

namespace Fl.Engine.Symbols
{
    public class ScopeChain
    {
        /// <summary>
        /// List of chained scopes
        /// </summary>
        private List<Scope> _Scopes;

        private int _Pointer;

        public ScopeChain()
        {
            _Scopes = new List<Scope>();
            _Pointer = -1;
        }

        private ScopeChain(List<Scope> scopes)
        {
            _Scopes = scopes;
            _Pointer = -1;
        }

        /// <summary>
        /// Returns the current scope in the chained scope list or the global scope
        /// </summary>
        public Scope CurrentScope => _Scopes.ElementAtOrDefault(_Pointer);

        /// <summary>
        /// Add a new scope to the scope's chain
        /// </summary>
        /// <param name="scopeType">Scope's type</param>
        public void EnterScope(ScopeType scopeType, string label = null, ScopeChain env = null)
        {
            if (_Scopes.Count == _Pointer + 1)
            {
                _Scopes.Add(new Scope(scopeType, label, env));
            }
            _Pointer++;
        }

        /// <summary>
        /// Destroys the current scope
        /// </summary>
        public void LeaveScope()
        {
            if (_Pointer > -1)
                _Pointer--;
        }

        #region Symbols handling        

        /// <summary>
        /// Returns a symbol from the current or the global scope
        /// </summary>
        /// <param name="name">Name of the symbol to lookup in the symbol table</param>
        /// <returns>FlObject containing information about symbol "name"</returns>
        public Symbol GetSymbol(string name)
        {
            int i = _Scopes.Count-1;
            var scp = _Scopes.ElementAtOrDefault(i);
            while (scp != null)
            {
                if (scp.HasSymbol(name))
                    return scp[name];

                if (scp.ScopeType == ScopeType.Function)
                    break;

                scp = _Scopes.ElementAtOrDefault(--i);
            }
            return null;
        }

        public Symbol GetSymbol(object name)
        {
            return this.GetSymbol(name.ToString());
        }

        /// <summary>
        /// Add a symbol to the current scope
        /// </summary>
        /// <param name="name">Name of the symbol to create</param>
        /// <param name="symbol">Contains attributes and information about the symbol</param>
        public void AddSymbol(string name, Symbol symbol, FlObject binding)
        {
            Scope current = CurrentScope;
            if (current.HasSymbol(name))
                throw new SymbolException($"Symbol '{name}' is already defined in this scope");
            
            current.AddSymbol(name, symbol, binding);
        }

        public bool HasSymbol(string var)
        {
            int i = _Scopes.Count - 1;
            var scp = _Scopes.ElementAtOrDefault(i);
            while (scp != null)
            {
                if (scp.HasSymbol(var))
                    return true;

                if (scp.ScopeType == ScopeType.Function)
                    return false;

                scp = _Scopes.ElementAtOrDefault(--i);
            }
            return false;
        }

        public void Using(FlNamespace ns)
        {
            CurrentScope.Using(ns);
        }
        #endregion

        #region Environments and binding
        public bool IsFunctionEnv()
        {
            return _Scopes.Count > 0 && _Scopes.Any(s => s.ScopeType == ScopeType.Function);
        }

        public ScopeChain GetCurrentFunctionEnv()
        {
            List<Scope> chain = new List<Scope>();
            int i = _Scopes.Count - 1;
            var scp = i >= 0 ? _Scopes[i] : null;
            while (scp != null)
            {
                chain.Add(scp);
                if (scp.ScopeType == ScopeType.Function)
                {
                    break;
                }
                scp = _Scopes.ElementAtOrDefault(--i);
            }
            return new ScopeChain(chain.Select(s => {
                var env = new Scope(s.ScopeType, s.Name);
                env.Import(s);
                return env;
            }).ToList());
        }
        #endregion

        #region Control Flow
        public FlObject ReturnValue
        {
            get
            {
                return CurrentScope[Scope.FlReturnKey].Binding;
            }
            set
            {
                int i = _Scopes.Count - 1;
                var scp = i >= 0 ? _Scopes[i] : null;
                while (scp != null)
                {
                    if (scp.ScopeType == ScopeType.Function)
                    {
                        scp.AddSymbol(Scope.FlReturnKey, new Symbol(SymbolType.Constant), value);
                        return;
                    }
                    scp = _Scopes.ElementAtOrDefault(--i);
                }
                throw new ScopeOperationException("Cannot return a value from a non-function scope");
            }
        }

        public bool MustReturn
        {
            get
            {
                int i = _Scopes.Count - 1;
                var scp = i >= 0 ? _Scopes[i] : null;
                while (scp != null)
                {
                    if (scp.ScopeType == ScopeType.Function && scp.HasSymbol(Scope.FlReturnKey))
                        return true;
                    scp = _Scopes.ElementAtOrDefault(--i);
                }
                return false;
            }
        }

        public bool MustBreak
        {
            get
            {
                int i = _Scopes.Count - 1;
                var scp = i >= 0 ? _Scopes[i] : null;
                while (scp != null)
                {
                    if (scp.ScopeType == ScopeType.Loop && scp.Break)
                        return true;
                    scp = _Scopes.ElementAtOrDefault(--i);
                }
                return false;
            }
        }

        public bool MustContinue
        {
            get
            {
                int i = _Scopes.Count - 1;
                var scp = i >= 0 ? _Scopes[i] : null;
                while (scp != null)
                {
                    if (scp.ScopeType == ScopeType.Loop && scp.Continue)
                        return true;
                    scp = _Scopes.ElementAtOrDefault(--i);
                }
                return false;
            }
        }

        public void SetBreak(FlInt intobj)
        {
            int nbreaks = intobj.Value;
            int orignbreaks = nbreaks;
            int i = _Scopes.Count - 1;
            var scp = i >= 0 ? _Scopes[i] : throw new ScopeOperationException("Cannot break in a non-loop scope");
            while (scp != null)
            {
                if (scp.ScopeType == ScopeType.Loop)
                {
                    scp.Break = true;
                    if (--nbreaks == 0)
                        break;
                }
                scp = _Scopes.ElementAtOrDefault(--i);
                if (scp == null && nbreaks >= 0)
                    throw new ScopeOperationException(nbreaks > 0 ? $"Cannot break more than {orignbreaks - nbreaks} loops" : "Cannot break in a non-loop scope");
            }
        }

        public void SetContinue()
        {
            int i = _Scopes.Count - 1;
            var scp = i >= 0 ? _Scopes[i] : throw new ScopeOperationException("Cannot continue in a non-loop scope");
            while (scp != null)
            {
                if (scp.ScopeType == ScopeType.Loop)
                {
                    scp.Continue = true;
                    break;
                }
                scp = _Scopes.ElementAtOrDefault(--i);
                if (scp == null)
                    throw new ScopeOperationException("Cannot continue in a non-loop scope");
            }
        }

        public void DoContinue()
        {
            if (CurrentScope.ScopeType != ScopeType.Loop)
                throw new ScopeOperationException("Cannot continue in a non-loop scope");
            CurrentScope.Continue = false;
        }
        #endregion
    }
}
