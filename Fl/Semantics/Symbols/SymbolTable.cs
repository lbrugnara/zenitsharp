// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fl.Semantics.Exceptions;
using Fl.Semantics.Inferrers;
using Fl.Semantics.Types;
using Fl.Syntax;

namespace Fl.Semantics.Symbols
{
    public class SymbolTable : ISymbolTable
    {
        /// <summary>
        /// Global scope is the first created (last in the stack) scope
        /// </summary>
        private ISymbolContainer Global { get; set; }

        /// <summary>
        /// Current scope is the latest added one
        /// </summary>
        public ISymbolContainer CurrentScope { get; private set; }

        private readonly Dictionary<string, List<Token>> unresolved;

        private TypeInferrer TypeInferrer { get; }

        public SymbolTable(TypeInferrer typeInferrer)
        {
            // Create the @global scope and set it as the current scope
            this.Global = this.CurrentScope = new Block("@global");
            this.TypeInferrer = typeInferrer;
            this.unresolved = new Dictionary<string, List<Token>>();
        }

        /// <summary>
        /// Check if there's a child scope in the current scope with the provided UID.
        /// If scope does not exist, create a new scope and chain it to the current scope.
        /// Either way, push retrieved/created scope to the stack to make it the current scope.
        /// </summary>
        /// <param name="type">Type of the scope to get/create</param>
        /// <param name="name">ID of the scope to get/create</param>
        public Block EnterBlockScope(string name)
        {
            return this.EnterBlockScope<Block>(name, this.CurrentScope);
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
            return this.EnterBlockScope<Loop>(name, this.CurrentScope);
        }

        private T EnterBlockScope<T>(string name, ISymbolContainer parent)
            where T : Block
        {
            T scope = parent.TryGet<T>(name);

            if (scope != null)
            {
                this.CurrentScope = scope;
                return scope;
            }

            if (typeof(T) == typeof(Loop))
                scope = new Loop(name, parent) as T;
            else if (typeof(T) == typeof(Block))
                scope = new Block(name, parent) as T;
            else
                throw new ScopeException($"Unknown scope type {typeof(T).Name}");

            parent.Insert(name, scope);
            this.CurrentScope = scope;
            return scope;
        }

        public FunctionSymbol EnterFunctionScope(string name)
        {
            return this.EnterComplexSymbolScope<FunctionSymbol>(name, this.CurrentScope);
        }

        public ObjectSymbol EnterObjectScope(string name)
        {
            return this.EnterComplexSymbolScope<ObjectSymbol>(name, this.CurrentScope);
        }

        public ClassSymbol EnterClassScope(string name)
        {
            return this.EnterComplexSymbolScope<ClassSymbol>(name, this.Global);
        }

        private T EnterComplexSymbolScope<T>(string name, ISymbolContainer parent)
            where T : ComplexSymbol
        {
            T scope = parent.TryGet<T>(name);

            if (scope != null)
            {
                this.CurrentScope = scope;
                return scope;
            }            

            if (typeof(T) == typeof(FunctionSymbol))
                scope = new FunctionSymbol(name, this.TypeInferrer.NewAnonymousType(), parent) as T;
            else if (typeof(T) == typeof(ObjectSymbol))
                scope = new ObjectSymbol(name, parent) as T;
            else if (typeof(T) == typeof(ClassSymbol))
                scope = new ClassSymbol(name, parent) as T;
            else
                throw new ScopeException($"Unknown scope type {typeof(T).Name}");

            parent.Insert(name, scope);
            this.CurrentScope = scope;

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
        public void LeaveScope()
        {
            if (this.CurrentScope.Parent == null)
                throw new ScopeOperationException($"Scope {this.CurrentScope.Name} does not have a parent scope");

            this.CurrentScope = this.CurrentScope.Parent;
        }

        /// <summary>
        /// Return true if the current scope (or its parent) is a ScopeType.Function
        /// </summary>
        public bool InFunction => this.CurrentScope is FunctionSymbol;

        #region ISymbolTable implementation

        /// <inheritdoc/>
        public void Insert(string name, IBoundSymbol symbol) => this.CurrentScope.Insert(name, symbol);

        public IBoundSymbol Insert(string name, ITypeSymbol type, Access access, Storage storage)
        {
            var symbol = new BoundSymbol(name, type, access, storage, this.CurrentScope);
            this.Insert(name, symbol);
            return symbol;
        }        

        public void Remove(string name) => this.CurrentScope.Remove(name);

        /// <inheritdoc/>
        public bool Contains(string name) => this.CurrentScope.Contains(name);

        /// <inheritdoc/>
        public IBoundSymbol GetBoundSymbol(string name) => this.CurrentScope.Get<IBoundSymbol>(name);

        public IBoundSymbol TryGetBoundSymbol(string name) => this.CurrentScope.Get<IBoundSymbol>(name);

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

            sb.AppendLine(this.Global.ToDumpString());

            return sb.ToString();
        }
    }
}
