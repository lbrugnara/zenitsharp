// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols.Exceptions;
using System.Collections.Generic;

namespace Fl.Symbols
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
        public ScopeType Type { get; }

        /// <summary>
        /// Contains symbols defined in this scope
        /// </summary>
        Dictionary<string, Symbol> Symbols { get; }

        /// <summary>
        /// Reference to the Global scope
        /// </summary>
        public Scope Global { get; private set; }

        /// <summary>
        /// If present, reference to the parent scope
        /// </summary>
        public Scope Parent { get; private set; }

        /// <summary>
        /// scope's children
        /// </summary>
        private Dictionary<string, Scope> Children { get; }


        public Scope(ScopeType type, string uid)
        {
            this.Uid = uid;
            this.Type = type;
            this.Symbols = new Dictionary<string, Symbol>();
            this.Children = new Dictionary<string, Scope>();

            if (type == ScopeType.Function)
                this.NewSymbol("@ret", null);
        }

        public Scope(ScopeType type, string uid, Scope global, Scope parent = null)
            : this(type, uid)
        {
            this.Global = global;
            if (parent != global)
                this.Parent = parent;
        }


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
                scope = new Scope(type, uid, this.Global, this);

                this.Children[uid] = scope;
            }

            return scope;
        }

        public bool IsFunction
        {
            get
            {
                return this.Type == ScopeType.Function || Parent != null && Parent.IsFunction;
            }
        }

        #region ISymbolTable implementation

        public void AddSymbol(Symbol symbol)
        {
            if (this.Symbols.ContainsKey(symbol.Name))
                throw new SymbolException($"Symbol {symbol.Name} is already defined in current scope");

            this.Symbols[symbol.Name] = symbol;
        }

        public Symbol NewSymbol(string name, Symbols.Types.Type type)
        {
            if (this.Symbols.ContainsKey(name))
                throw new SymbolException($"Symbol {name} is already defined in current scope");

            var symbol = new Symbol(name, type);
            this.Symbols[name] = symbol;
            return symbol;
        }

        public bool HasSymbol(string name) => this.Symbols.ContainsKey(name) || (this.Parent != null && this.Parent.HasSymbol(name)) || (this.Global != null && this.Global.HasSymbol(name));

        public Symbol GetSymbol(string name) => this.TryGetSymbol(name) ?? throw new SymbolException($"Symbol {name} is not defined in current scope");

        public Symbol TryGetSymbol(string name) =>
            this.Symbols.ContainsKey(name)
            ? this.Symbols[name]
            : this.Parent != null && this.Parent.HasSymbol(name)
                ? this.Parent.TryGetSymbol(name)
                : this.Global != null && this.Global.HasSymbol(name)
                    ? this.Global.TryGetSymbol(name)
                    : null;

        #endregion
    }
}
