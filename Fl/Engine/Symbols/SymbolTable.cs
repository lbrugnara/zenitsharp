// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Fl.Engine.Symbols.Exceptions;

namespace Fl.Engine.Symbols
{
    public class SymbolTable
    {
        /// <summary>
        /// Reference ot the global scope
        /// </summary>
        private Scope _Global;

        /// <summary>
        /// List of chained scopes
        /// </summary>
        private List<Scope> _Scopes;

        public SymbolTable()
        {
            _Scopes = new List<Scope>();
            _Global = new Scope(ScopeType.Common);
            // Initialize the global scope with the standard lib
            StdLibInitializer.Import(CurrentScope);
        }

        /// <summary>
        /// Returns the current scope in the chained scope list or the global scope
        /// </summary>
        public Scope CurrentScope => _Scopes.LastOrDefault() ?? _Global;

        /// <summary>
        /// Returns the global scope
        /// </summary>
        public Scope GlobalScope => _Global;

        /// <summary>
        /// Add a new scope to the scope's chain
        /// </summary>
        /// <param name="scopeType">Scope's type</param>
        public void NewScope(ScopeType scopeType, Scope env = null)
        {
            _Scopes.Add(new Scope(scopeType, env));
        }

        /// <summary>
        /// Destroys the current scope
        /// </summary>
        public void DestroyScope()
        {
            _Scopes.RemoveAt(_Scopes.Count-1);
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
            var scp = i >= 0 ? _Scopes[i] : _Global;
            while (scp != null)
            {
                if (scp.HasSymbol(name))
                    return scp[name];

                if (scp == _Global)
                    break;

                if (scp.ScopeType == ScopeType.Function)
                    return _Global.HasSymbol(name) ? _Global[name] : null;

                scp = _Scopes.ElementAtOrDefault(--i) ?? _Global;
            }
            throw new SymbolException($"Symbol '{name}' does not exist in the current context");
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
        public void AddSymbol(string name, Symbol symbol)
        {
            Scope current = CurrentScope;
            if (current.HasSymbol(name))
                throw new SymbolException($"Symbol '{name}' is already defined in this scope");
            
            current.AddSymbol(name, symbol);
        }

        /// <summary>
        /// Updates a symbol with new information
        /// </summary>
        /// <param name="name">Symbol name to update</param>
        /// <param name="obj">New attributes or information for the target symbol</param>
        public void UpdateSymbol(string name, FlObject obj)
        {
            int i = _Scopes.Count - 1;
            var scp = i >= 0 ? _Scopes[i] : _Global;
            while (scp != null)
            {
                if (scp.HasSymbol(name))
                {
                    scp.UpdateSymbol(name, obj);
                    return;
                }

                if (scp == _Global)
                    break;

                if (scp.ScopeType == ScopeType.Function)
                {
                    if (!_Global.HasSymbol(name))
                        break;
                    _Global.UpdateSymbol(name, obj);
                    return;
                }
                scp = _Scopes.ElementAtOrDefault(--i) ?? _Global;
            }
            throw new SymbolException($"Symbol '{name}' does not exist in the current context");
        }

        public bool HasSymbol(string var)
        {
            int i = _Scopes.Count - 1;
            var scp = i >= 0 ? _Scopes[i] : _Global;
            while (scp != null)
            {
                if (scp.HasSymbol(var))
                    return true;

                if (scp == _Global)
                    break;

                if (scp.ScopeType == ScopeType.Function)
                {
                    return _Global.HasSymbol(var);
                }

                scp = _Scopes.ElementAtOrDefault(--i) ?? _Global;
            }
            return false;
        }

        public void Import(Scope s)
        {
            CurrentScope.Import(s);
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

        public Scope GetCurrentFunctionEnv()
        {
            int i = _Scopes.Count - 1;
            var scp = i >= 0 ? _Scopes[i] : null;
            while (scp != null)
            {
                if (scp.ScopeType == ScopeType.Function)
                {
                    var env = new Scope(ScopeType.Function);
                    env.Import(scp);
                    return env;
                }
                scp = _Scopes.ElementAtOrDefault(--i);
            }
            return null;
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
                        scp.AddSymbol(Scope.FlReturnKey, new Symbol(value.Type, StorageType.Constant, value));
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

        public void SetBreak(int nbreaks)
        {
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
