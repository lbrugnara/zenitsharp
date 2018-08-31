// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;
using System.Linq;
using Fl.Semantics.Exceptions;
using Fl.Semantics.Types;
using Fl.Syntax;

namespace Fl.Semantics.Symbols
{
    public class SymbolTable : ISymbolTable
    {
        /// <summary>
        /// Keep track of the scope chain
        /// </summary>
        private readonly Stack<Scope> scopes;

        private readonly Dictionary<string, List<Token>> unresolved;

        public SymbolTable()
        {
            this.scopes = new Stack<Scope>();

            // Create the initial scope (Global)
            this.scopes.Push(new Scope(ScopeType.Global, "global"));
            this.unresolved = new Dictionary<string, List<Token>>();
        }

        /// <summary>
        /// Global scope is the first created (last in the stack) scope
        /// </summary>
        private Scope Global => this.scopes.Last();

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

        public void EnterClassScope(string className)
        {
            if (this.CurrentScope.IsPackage)
            {
                // TODO: Do something with the Package
            }
            if (this.CurrentScope.IsClass)
            {
                // TODO: Do something with the Class
            }
            else
            {
                this.scopes.Push(this.Global.GetOrCreateNestedScope(ScopeType.Class, className));
            }
        }

        public Scope GetClassScope(string className)
        {
            if (this.CurrentScope.IsPackage)
            {
                // TODO: Do something with the Package
            }
            if (this.CurrentScope.IsClass)
            {
                // TODO: Do something with the Class
            }

            return this.Global.GetNestedScope(ScopeType.Class, className);
        }

        public Scope GetFunctionScope(string funcName)
        {
            if (this.CurrentScope.IsPackage)
            {
                // TODO: Do something with the Package
            }
            if (this.CurrentScope.IsClass)
            {
                // TODO: Do something with the Class
            }

            return this.Global.GetNestedScope(ScopeType.Function, funcName);
        }

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
        public Symbol NewSymbol(string name, Type type, Access access, Storage storage) => this.scopes.Peek().NewSymbol(name, type, access, storage);

        /// <inheritdoc/>
        public bool HasSymbol(string name) => this.scopes.Peek().HasSymbol(name);

        /// <inheritdoc/>
        public Symbol GetSymbol(string name) => this.scopes.Peek().GetSymbol(name);

        public Symbol TryGetSymbol(string name) => this.scopes.Peek().TryGetSymbol(name);

        #endregion

        public Symbol NewClassSymbol(string className, Class clasz, Access access)
        {
            if (this.unresolved.ContainsKey(className))
                this.unresolved.Remove(className);

            Scope scope = this.Global;

            if (this.CurrentScope.IsPackage)
            {
                // TODO: Do something with the Package
            }
            if (this.CurrentScope.IsClass)
            {
                // TODO: Do something with the Class
            }

            return scope.NewSymbol(className, clasz, access, Storage.Constant);
        }

        public void AddUnresolvedType(string name, Token info)
        {
            if (!this.unresolved.ContainsKey(name))
                this.unresolved[name] = new List<Token>();
            this.unresolved[name].Add(info);
        }

        public void ThrowIfUnresolved()
        {
            if (this.unresolved.Count == 0)
                return;

            var errors = new List<string>();

            foreach (var type in this.unresolved)
            {
                var name = type.Key;
                var tokens = type.Value;

                tokens.ForEach(token => errors.Add($"Type '{name}' is not defined in line {token.Line}:{token.Col}"));
            }

            throw new SymbolException(string.Join("\n", errors));
        }
    }
}
