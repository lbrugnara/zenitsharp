// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Objects;

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
        private ScopeChain _Scopes;

        private SymbolTable()
        {
            _Scopes = new ScopeChain();
            _Global = new Scope(ScopeType.Common, "<global>");
            _Global.AddSymbol("@return", new Symbol(SymbolType.Variable, StorageType.Auto), null);

            // Initialize the global scope with the standard lib
            StdLibInitializer.Import(CurrentScope);
        }

        private static SymbolTable _Instance;

        public static SymbolTable Instance => (_Instance == null ? (_Instance = new SymbolTable()) : _Instance);

        public static SymbolTable NewInstance => new SymbolTable();

        /// <summary>
        /// Returns the current scope in the chained scope list or the global scope
        /// </summary>
        public Scope CurrentScope => _Scopes.CurrentScope ?? _Global;

        /// <summary>
        /// Returns the global scope
        /// </summary>
        public Scope GlobalScope => _Global;

        /// <summary>
        /// Add a new scope to the scope's chain
        /// </summary>
        /// <param name="scopeType">Scope's type</param>
        public void EnterScope(ScopeType scopeType, string name = null, ScopeChain env = null)
        {
            _Scopes.EnterScope(scopeType, name, env);
        }

        /// <summary>
        /// Destroys the current scope
        /// </summary>
        public void LeaveScope()
        {
            _Scopes.LeaveScope();
        }

        #region Symbols handling        

        /// <summary>
        /// Returns a symbol from the current or the global scope
        /// </summary>
        /// <param name="name">Name of the symbol to lookup in the symbol table</param>
        /// <returns>FlObject containing information about symbol "name"</returns>
        public Symbol GetSymbol(string name)
        {
            return 
                _Scopes?.GetSymbol(name) 
                ?? (_Global.HasSymbol(name) ? _Global[name] : null) 
                ?? throw new SymbolException($"Symbol '{name}' does not exist in the current context");
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
            Scope current = _Scopes.CurrentScope ?? _Global;
            /*if (current.HasSymbol(name))
                throw new SymbolException($"Symbol '{name}' is already defined in this scope");*/
            
            current.AddSymbol(name, symbol, binding);
        }

        public bool HasSymbol(string var)
        {
            if (_Scopes.CurrentScope != null && _Scopes.CurrentScope.HasSymbol(var))
                return true;
            return _Global.HasSymbol(var);
        }

        public void Using(FlNamespace ns)
        {
            CurrentScope.Using(ns);
        }
        #endregion

        #region Environments and binding
        public bool IsFunctionEnv()
        {
            return _Scopes.IsFunctionEnv();
        }

        public ScopeChain GetCurrentFunctionEnv()
        {
            return _Scopes.GetCurrentFunctionEnv();
        }
        #endregion

        #region Control Flow
        public FlObject ReturnValue
        {
            get
            {
                //return _Scopes.ReturnValue;
                return _Global["@return"]?.Binding;
            }
            set
            {
                //_Scopes.ReturnValue = value;
                _Global["@return"].UpdateBinding(value);
            }
        }

        public bool MustReturn
        {
            get
            {
                return _Scopes.MustReturn;
            }
        }

        public bool MustBreak
        {
            get
            {
                return _Scopes.MustBreak;
            }
        }

        public bool MustContinue
        {
            get
            {
                return _Scopes.MustBreak;
            }
        }

        public void SetBreak(FlInt intobj)
        {
            _Scopes.SetBreak(intobj);
        }

        public void SetContinue()
        {
            _Scopes.SetContinue();
        }

        public void DoContinue()
        {
            _Scopes.DoContinue();
        }
        #endregion
    }
}
