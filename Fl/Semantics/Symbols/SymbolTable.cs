// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;
using System.Linq;
using Fl.Semantics.Types;

namespace Fl.Semantics.Symbols
{
    public class SymbolTable : ISymbolTable
    {
        /// <summary>
        /// An stack to keep track of nested scopes
        /// </summary>
        private readonly Stack<Scope> scopes;

        public SymbolTable()
        {
            this.scopes = new Stack<Scope>();

            // Create the initial scope (Global)
            this.scopes.Push(new Scope(ScopeType.Global, "global"));
        }

        /// <summary>
        /// Global scope is the first created (last in the stack) scope
        /// </summary>
        public Scope Global => this.scopes.Last();

        /// <summary>
        /// Current scope is the latest added one
        /// </summary>
        public Scope CurrentScope => this.scopes.Peek();

        /// <summary>
        /// Check if there's a child scope in the current scope with the provided UID.
        /// If scope does not exist, create a new scope and chain it to the current scope.
        /// Either way, push retrieved/created scope to the stack to make it the current scope.
        /// </summary>
        /// <param name="type">Type of the scope to get/create</param>
        /// <param name="uid">ID of the scope to get/create</param>
        public void EnterScope(ScopeType type, string uid) => this.scopes.Push(this.scopes.Peek().GetOrCreateNestedScope(type, uid));

        /// <summary>
        /// Enter to the function's scope, chain it to the stack of scopes, and make it the current 
        /// executing scope
        /// </summary>
        /// <param name="function">Function symbol owner of the scope</param>
        //public void EnterFunctionScope(FunctionSymbol funcsym) => this.scopes.Push(funcsym.Scope);

        /// <summary>
        /// Remove the current scope from the stack (go back to the current scope's parent)
        /// </summary>
        public void LeaveScope() => this.scopes.Pop();

        /// <summary>
        /// Return true if the current scope (or its parent) is a ScopeType.Function
        /// </summary>
        public bool InFunction => this.scopes.Peek().IsFunction;

        #region ISymbolTable implementation

        /// <inheritdoc/>
        public void AddSymbol(Symbol symbol) => this.scopes.Peek().AddSymbol(symbol);

        /// <inheritdoc/>
        public Symbol NewSymbol(string name, Type type) => this.scopes.Peek().NewSymbol(name, type);

        /// <inheritdoc/>
        public bool HasSymbol(string name) => this.scopes.Peek().HasSymbol(name);

        /// <inheritdoc/>
        public Symbol GetSymbol(string name) => this.scopes.Peek().GetSymbol(name);

        public Symbol TryGetSymbol(string name) => this.scopes.Peek().TryGetSymbol(name);

        #endregion
    }
}
