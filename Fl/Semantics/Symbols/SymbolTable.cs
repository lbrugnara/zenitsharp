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
    public class SymbolTable : ISymbolTable
    {
        /// <summary>
        /// Keep track of the scope chain
        /// </summary>
        private readonly Stack<ISymbolContainer> scopes;

        private readonly Dictionary<string, List<Token>> unresolved;

        public SymbolTable()
        {
            this.scopes = new Stack<ISymbolContainer>();

            // Create the initial scope (Global)
            this.scopes.Push(new Block("@global"));
            this.unresolved = new Dictionary<string, List<Token>>();
        }

        /// <summary>
        /// Global scope is the first created (last in the stack) scope
        /// </summary>
        private ISymbolContainer Global => this.scopes.Last();

        /// <summary>
        /// Current scope is the latest added one
        /// </summary>
        public ISymbolContainer CurrentScope => this.scopes.Peek();

        /// <summary>
        /// Check if there's a child scope in the current scope with the provided UID.
        /// If scope does not exist, create a new scope and chain it to the current scope.
        /// Either way, push retrieved/created scope to the stack to make it the current scope.
        /// </summary>
        /// <param name="type">Type of the scope to get/create</param>
        /// <param name="name">ID of the scope to get/create</param>
        public Block EnterBlockScope(string name)
        {
            return this.EnterBlockScope<Block>(name, this.scopes.Peek());
        }

        /// <summary>
        /// Check if there's a child scope in the current scope with the provided UID.
        /// If scope does not exist, create a new scope and chain it to the current scope.
        /// Either way, push retrieved/created scope to the stack to make it the current scope.
        /// </summary>
        /// <param name="type">Type of the scope to get/create</param>
        /// <param name="name">ID of the scope to get/create</param>
        public Loop EnterLoopScope(string name)
        {
            return this.EnterBlockScope<Loop>(name, this.scopes.Peek());
        }

        private T EnterBlockScope<T>(string name, ISymbolContainer parent)
            where T : Block
        {
            T scope = null;

            if (parent.Contains(name))
            {
                scope = parent.Get<T>(name);
                this.scopes.Push(scope);
                return scope;
            }

            if (typeof(T) == typeof(Loop))
                scope = new Loop(name, parent) as T;
            else if (typeof(T) == typeof(Block))
                scope = new Block(name, parent) as T;
            else
                throw new ScopeException($"Unknown scope type {typeof(T).Name}");

            parent.Add(scope);
            this.scopes.Push(scope);

            return scope;
        }

        public FunctionSymbol EnterFunctionScope(string name)
        {
            return this.EnterComplexSymbolScope<FunctionSymbol>(name, this.scopes.Peek());
        }

        public ObjectSymbol EnterObjectScope(string name)
        {
            return this.EnterComplexSymbolScope<ObjectSymbol>(name, this.scopes.Peek());
        }

        public ClassSymbol EnterClassScope(string name)
        {
            return this.EnterComplexSymbolScope<ClassSymbol>(name, this.Global);
        }

        private T EnterComplexSymbolScope<T>(string name, ISymbolContainer parent)
            where T : ComplexSymbol
        {
            T scope = null;

            if (parent.Contains(name))
            {
                scope = parent.Get<T>(name);
                this.scopes.Push(scope);
                return scope;
            }            

            if (typeof(T) == typeof(FunctionSymbol))
                scope = new FunctionSymbol(name, parent) as T;
            else if (typeof(T) == typeof(ObjectSymbol))
                scope = new ObjectSymbol(name, parent) as T;
            else if (typeof(T) == typeof(ClassSymbol))
                scope = new ClassSymbol(name, parent) as T;
            else
                throw new ScopeException($"Unknown scope type {typeof(T).Name}");

            parent.Add(scope);
            this.scopes.Push(scope);

            return scope;
        }

        public ClassSymbol GetClassScope(string className)
        {
            // TODO: Handle Package and nested classes
            return this.Global.Get<ClassSymbol>(className);
        }

        public FunctionSymbol GetFunctionScope(string funcName)
        {
            return this.CurrentScope.Get<FunctionSymbol>(funcName);
        }

        /// <summary>
        /// Remove the current scope from the stack (go back to the current scope's parent)
        /// </summary>
        public void LeaveScope() => this.scopes.Pop();

        /// <summary>
        /// Return true if the current scope (or its parent) is a ScopeType.Function
        /// </summary>
        public bool InFunction => this.scopes.Peek() is FunctionSymbol;

        #region ISymbolTable implementation

        public ISymbol Insert(string name, TypeInfo type, Access access, Storage storage)
        {
            var symbol = new Symbol(name, type, access, storage, this.CurrentScope);
            this.Insert(symbol);
            return symbol;
        }

        /// <inheritdoc/>
        public void Insert(ISymbol symbol) => this.scopes.Peek().Add(symbol);

        /// <inheritdoc/>
        public bool Contains(string name) => this.scopes.Peek().Contains(name);

        /// <inheritdoc/>
        public ISymbol Lookup(string name) => this.scopes.Peek().Get<ISymbol>(name);

        public ISymbol TryLookup(string name) => this.scopes.Peek().Get<ISymbol>(name);

        #endregion

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
