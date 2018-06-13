// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;
using System.Linq;
using Fl.Symbols.Types;

namespace Fl.Symbols
{
    public class SymbolTable : ISymbolTable
    {
        /// <summary>
        /// An stack to keep track of nested scopes
        /// </summary>
        private Stack<Scope> Scopes { get; }

        public SymbolTable()
        {
            this.Scopes = new Stack<Scope>();

            // Create the initial scope (Global)
            this.Scopes.Push(new Scope(ScopeType.Global, "global"));
        }

        /// <summary>
        /// Global scope is the first created (last in the stack) scope
        /// </summary>
        public Scope Global => this.Scopes.Last();

        /// <summary>
        /// Check if there's a child scope in the current scope with the provided UID.
        /// If scope does not exist, create a new scope and chain it to the current scope.
        /// Either way, push retrieved/created scope to the stack to make it the current scope.
        /// </summary>
        /// <param name="type">Type of the scope to get/create</param>
        /// <param name="uid">ID of the scope to get/create</param>
        public void EnterScope(ScopeType type, string uid) => this.Scopes.Push(this.Scopes.Peek().GetOrCreateNestedScope(type, uid));

        /// <summary>
        /// Enter to the function's scope, chain it to the stack of scopes, and make it the current 
        /// executing scope
        /// </summary>
        /// <param name="function">Function symbol owner of the scope</param>
        public void EnterFunctionScope(Function function) => this.Scopes.Push(function.Scope);

        /// <summary>
        /// Remove the current scope from the stack (go back to the current scope's parent)
        /// </summary>
        public void LeaveScope() => this.Scopes.Pop();

        public bool InFunction => this.Scopes.Peek().IsFunction;

        #region ISymbolTable implementation

        /// <inheritdoc/>
        public void AddSymbol(Symbol symbol) => this.Scopes.Peek().AddSymbol(symbol);

        /// <inheritdoc/>
        public Symbol NewSymbol(string name, Type type) => this.Scopes.Peek().NewSymbol(name, type);

        /// <inheritdoc/>
        public bool HasSymbol(string name) => this.Scopes.Peek().HasSymbol(name);

        /// <inheritdoc/>
        public Symbol GetSymbol(string name) => this.Scopes.Peek().GetSymbol(name);

        #endregion
    }
}
