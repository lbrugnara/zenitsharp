// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Exceptions;
using Fl.Semantics.Types;
using System.Collections.Generic;
using System.Linq;

namespace Fl.Semantics.Symbols
{
    public class Scope : ISymbolTable
    {
        /// <summary>
        /// Scope unique id used in mangled names
        /// </summary>
        public string Uid { get; }

        /// <summary>
        /// Type of the current scope
        /// </summary>
        public virtual ScopeType Type { get; set; }

        /// <summary>
        /// Contains symbols defined in this scope
        /// </summary>
        Dictionary<string, Symbol> Symbols { get; }

        /// <summary>
        /// Reference to the Global scope
        /// </summary>
        protected Scope Global { get; set; }

        /// <summary>
        /// If present, reference to the parent scope
        /// </summary>
        protected Scope Parent { get; set; }

        /// <summary>
        /// scope's children
        /// </summary>
        protected Dictionary<string, Scope> Children { get; }

        /// <summary>
        /// This constructor creates an instance that represents the Global scope, as caller
        /// is not providing a prent scope
        /// </summary>
        /// <param name="uid">Scope's UID</param>
        public Scope(string uid)
        {
            this.Uid = uid;
            this.Type = ScopeType.Global;
            this.Symbols = new Dictionary<string, Symbol>();
            this.Children = new Dictionary<string, Scope>();            
        }

        /// <summary>
        /// If the parent scope is present, caller is creating a Common scope.
        /// We navigate through the chain to get a reference to the global scope
        /// </summary>
        /// <param name="uid">Scope's UID</param>
        /// <param name="parent">Parent scope of the new instance</param>
        public Scope(string uid, Scope parent)
            : this(uid)
        {
            this.Type = ScopeType.Common;
            this.Parent = parent;

            var current = parent;
            while (current.Parent != null)
                current = current.Parent;

            this.Global = current;
        }

        /// <summary>
        /// If the scope with the specified type and UID exists as a child of the current scope, that instance
        /// is returned.
        /// In case the requested scope does not exist, this method creates a new one, saves it as a child, and returns
        /// the new nested scope
        /// </summary>
        /// <param name="type">Child scope's type</param>
        /// <param name="uid">Child scope's UID</param>
        /// <returns>The child scope with the specified type and UID</returns>
        public Scope GetOrCreateNestedScope(ScopeType type, string uid)
        {
            Scope scope = null;

            if (this.Children.ContainsKey(uid))
            {
                scope = this.Children[uid];

                if (scope.Type != type)
                    throw new ScopeException($"Expecting scope {uid} to be of type {type} but it has type {scope.Type}");
            }
            else
            {
                switch (type)
                {
                    case ScopeType.Function:
                        scope = new FunctionScope(uid, this);
                        break;
                    case ScopeType.Class:
                        scope = new ClassScope(uid, this);
                        break;
                    case ScopeType.Object:
                        scope = new ObjectScope(uid, this);
                        break;
                    default:
                        scope = new Scope(uid, this);
                        break;
                }                

                this.Children[uid] = scope;
            }

            return scope;
        }

        /// <summary>
        /// This method returns (if exists) a child scope with the requested type and UID.
        /// If the child does not exist or the type missmatches, this method throws an ScopeException
        /// </summary>
        /// <param name="type">Child scope's type</param>
        /// <param name="uid">Child scope's UID</param>
        /// <returns>The child scope with the specified type and UID</returns>
        public Scope GetNestedScope(ScopeType type, string uid)
        {
            Scope scope = null;

            if (!this.Children.ContainsKey(uid))
                throw new ScopeException($"Scope {uid} ({type}) does not exist");
            
            scope = this.Children[uid];

            if (scope.Type != type)
                throw new ScopeException($"Expecting scope {uid} to be of type {type} but it has type {scope.Type}");

            return scope;
        }

        /// <summary>
        /// Returns true if the chain scope refers to the Global scope.
        /// If in the chain there is a function, package, or class scope, this function returns false,
        /// true otherwise
        /// </summary>
        public bool IsGlobal
        {
            get
            {
                if (this.Type == ScopeType.Global)
                    return true;

                var scope = this.Parent ?? this.Global;

                while (scope != null)
                {
                    if (scope.Type == ScopeType.Global)
                        return true;

                    if (scope.IsFunction || scope.IsPackage || scope.IsClass)
                        return false;

                    scope = scope.Parent ?? scope.Global;
                }

                return this.Type == ScopeType.Global || Parent != null && Global != Parent && Parent.IsGlobal;
            }
        }

        /// <summary>
        /// Returns true if the current scope or an ancestor is a Function scope
        /// </summary>
        public bool IsFunction
        {
            get
            {
                if (this.Type == ScopeType.Function)
                    return true;

                if (this.Parent == null)
                    return false;

                var scope = this.Parent;

                while (scope != null)
                {
                    if (scope.Type == ScopeType.Function)
                        return true;

                    if (scope.IsPackage || scope.IsClass)
                        return false;

                    scope = scope.Parent;
                }

                return false;
            }
        }

        /// <summary>
        /// Returns true if the current scope or an ancestor is a Package scope
        /// </summary>
        public bool IsPackage
        {
            get
            {
                if (this.Type == ScopeType.Package)
                    return true;

                if (this.Parent == null)
                    return false;

                var scope = this.Parent;

                while (scope != null)
                {
                    if (scope.Type == ScopeType.Package)
                        return true;

                    if (scope.IsFunction || scope.IsClass)
                        return false;

                    scope = scope.Parent;
                }

                return false;
            }
        }

        /// <summary>
        /// Returns true if the current scope or an ancestor is a Class scope
        /// </summary>
        public bool IsClass
        {
            get
            {
                if (this.Type == ScopeType.Class)
                    return true;

                if (this.Parent == null)
                    return false;

                var scope = this.Parent;

                while (scope != null)
                {
                    if (scope.Type == ScopeType.Class)
                        return true;

                    if (scope.IsFunction || scope.IsPackage)
                        return false;

                    scope = scope.Parent;
                }

                return false;
            }
        }

        /// <summary>
        /// Returns true if the current scope or an ancestor is a Function scope
        /// </summary>
        public FunctionScope CurrentFunction
        {
            get
            {
                if (this.Type == ScopeType.Function)
                    return this as FunctionScope;

                if (this.Parent == null)
                    throw new ScopeOperationException("Current scope is not a function");

                var scope = this.Parent;

                while (scope != null)
                {
                    if (scope.Type == ScopeType.Function)
                        return scope as FunctionScope;

                    if (scope.IsPackage || scope.IsClass)
                        break;

                    scope = scope.Parent;
                }

                throw new ScopeOperationException("Current scope is not a function");
            }
        }

        #region ISymbolTable implementation

        public void AddSymbol(Symbol symbol)
        {
            if (this.Symbols.ContainsKey(symbol.Name))
                throw new SymbolException($"Symbol {symbol.Name} is already defined in current scope");

            this.Symbols[symbol.Name] = symbol;
        }

        public Symbol CreateSymbol(string name, TypeInfo type, Access access, Storage storage)
        {
            if (this.Symbols.ContainsKey(name))
                throw new SymbolException($"Symbol {name} is already defined in current scope");

            return this.Symbols[name] = new Symbol(name, type, access, storage);
        }

        public List<Symbol> GetAllSymbols() => this.Symbols.Values.ToList();

        public bool HasSymbol(string name) 
            => this.Symbols.ContainsKey(name) 
            || (this.Parent != null && this.Parent.HasSymbol(name))/* 
            || (this.Type != ScopeType.Global && this.Global != null && this.Global.HasSymbol(name))*/;

        public Symbol GetSymbol(string name) 
            => this.TryGetSymbol(name) ?? throw new SymbolException($"Symbol {name} is not defined in current scope");

        public Symbol TryGetSymbol(string name) =>
            this.Symbols.ContainsKey(name)
            ? this.Symbols[name]
            : this.Parent != null && this.Parent.HasSymbol(name)
                ? this.Parent.TryGetSymbol(name)
                : /*this.Global != null && this.Global.HasSymbol(name)
                    ? this.Global.TryGetSymbol(name)
                    :*/ null;

        #endregion
    }
}
