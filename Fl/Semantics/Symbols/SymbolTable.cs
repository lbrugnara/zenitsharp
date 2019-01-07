// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fl.Semantics.Exceptions;
using Fl.Semantics.Types;
using Fl.Syntax;

namespace Fl.Semantics.Symbols
{
    public class SymbolTable : ISymbolContainer
    {
        /// <summary>
        /// Keep track of the scope chain
        /// </summary>
        private readonly Stack<SymbolContainer> scopes;

        private readonly Dictionary<string, List<Token>> unresolved;

        public SymbolTable()
        {
            this.scopes = new Stack<SymbolContainer>();

            // Create the initial scope (Global)
            this.scopes.Push(new BlockSymbolContainer("@global"));
            this.unresolved = new Dictionary<string, List<Token>>();
        }

        /// <summary>
        /// Global scope is the first created (last in the stack) scope
        /// </summary>
        private SymbolContainer Global => this.scopes.Last();

        /// <summary>
        /// Current scope is the latest added one
        /// </summary>
        public SymbolContainer CurrentScope => this.scopes.Peek();

        /// <summary>
        /// Check if there's a child scope in the current scope with the provided UID.
        /// If scope does not exist, create a new scope and chain it to the current scope.
        /// Either way, push retrieved/created scope to the stack to make it the current scope.
        /// </summary>
        /// <param name="type">Type of the scope to get/create</param>
        /// <param name="name">ID of the scope to get/create</param>
        public SymbolContainer EnterBlockScope(string name)
        {
            return this.EnterScope<BlockSymbolContainer>(name, this.scopes.Peek());
        }

        /// <summary>
        /// Check if there's a child scope in the current scope with the provided UID.
        /// If scope does not exist, create a new scope and chain it to the current scope.
        /// Either way, push retrieved/created scope to the stack to make it the current scope.
        /// </summary>
        /// <param name="type">Type of the scope to get/create</param>
        /// <param name="name">ID of the scope to get/create</param>
        public LoopSymbolContainer EnterLoopScope(string name)
        {
            return this.EnterScope<LoopSymbolContainer>(name, this.scopes.Peek());
        }

        public FunctionSymbol EnterFunctionScope(string name)
        {
            return this.EnterScope<FunctionSymbol>(name, this.scopes.Peek());
        }

        public ObjectSymbol EnterObjectScope(string name)
        {
            return this.EnterScope<ObjectSymbol>(name, this.scopes.Peek());
        }

        public ClassSymbol EnterClassScope(string name)
        {
            return this.EnterScope<ClassSymbol>(name, this.Global);
        }

        private T EnterScope<T>(string name, SymbolContainer currentScope)
            where T : SymbolContainer
        {
            T targetScope = null;

            if (currentScope.HasChild(name))
            {
                targetScope = currentScope.GetChild(name) as T;
            }
            else
            {
                targetScope = Activator.CreateInstance(typeof(T), name, currentScope) as T;
                currentScope.AddChild(targetScope);
            }

            this.scopes.Push(targetScope);

            return targetScope;
        }
        public SymbolContainer GetClassScope(string className)
        {
            // TODO: Handle Package and nested classes
            return this.Global.GetChild(className);
        }

        public FunctionSymbol GetFunctionScope(string funcName)
        {
            return this.CurrentScope.GetChild(funcName) as FunctionSymbol;
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
        public bool InFunction => this.scopes.Peek() is FunctionSymbol;

        #region ISymbolTable implementation

        /// <inheritdoc/>
        public void AddSymbol(Symbol symbol) 
            => this.scopes.Peek().AddSymbol(symbol);

        /// <inheritdoc/>
        public Symbol CreateSymbol(string name, TypeInfo type, Access access, Storage storage) 
            => this.scopes.Peek().CreateSymbol(name, type, access, storage);

        /// <inheritdoc/>
        public bool HasSymbol(string name) => this.scopes.Peek().HasSymbol(name);

        /// <inheritdoc/>
        public Symbol GetSymbol(string name) => this.scopes.Peek().GetSymbol(name);

        public Symbol TryGetSymbol(string name) => this.scopes.Peek().TryGetSymbol(name);

        #endregion

        public Symbol NewClassSymbol(string className, TypeInfo clasz, Access access)
        {
            if (this.unresolved.ContainsKey(className))
                this.unresolved.Remove(className);

            SymbolContainer scope = this.Global;

            /*if (this.CurrentScope.IsPackage)
            {
                // TODO: Do something with the Package
            }
            if (this.CurrentScope.IsClass)
            {
                // TODO: Do something with the Class
            }*/

            return scope.CreateSymbol(className, clasz, access, Storage.Constant);
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

        public string ToDebugString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("[Symbol Table]");

            sb.AppendLine(this.Global.ToDebugString(1));

            return sb.ToString();
        }
    }
}
