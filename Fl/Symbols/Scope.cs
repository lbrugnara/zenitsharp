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
        /// Type of the current block
        /// </summary>
        public ScopeType Type { get; }

        /// <summary>
        /// Contains symbols defined in this block
        /// </summary>
        Dictionary<string, Symbol> Symbols { get; }

        /// <summary>
        /// Reference to the Global block
        /// </summary>
        public Scope Global { get; private set; }

        /// <summary>
        /// If present, reference to the parent block
        /// </summary>
        public Scope Parent { get; private set; }

        /// <summary>
        /// Block's children
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
            Scope block = null;

            if (this.Children.ContainsKey(uid))
            {
                block = this.Children[uid];

                if (block.Type != type)
                    throw new ScopeException($"Expecting block {uid} to be of type {type} but it has type {block.Type}");
            }
            else
            {
                block = new Scope(type, uid, this.Global, this);

                this.Children[uid] = block;
            }

            return block;
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
                throw new SymbolException($"Symbol {symbol.Name} is already defined in current block");

            this.Symbols[symbol.Name] = symbol;
        }

        public Symbol NewSymbol(string name, Symbols.Types.SType type)
        {
            if (this.Symbols.ContainsKey(name))
                throw new SymbolException($"Symbol {name} is already defined in current block");

            var symbol = new Symbol(name, type, this.Uid);
            this.Symbols[name] = symbol;
            return symbol;
        }

        public bool HasSymbol(string name) => this.Symbols.ContainsKey(name) || (this.Parent != null && this.Parent.HasSymbol(name)) || (this.Global != null && this.Global.HasSymbol(name));

        public Symbol GetSymbol(string name) =>
            this.Symbols.ContainsKey(name)
            ? this.Symbols[name]
            : this.Parent != null && this.Parent.HasSymbol(name)
                ? this.Parent.GetSymbol(name)
                : this.Global != null && this.Global.HasSymbol(name)
                    ? this.Global.GetSymbol(name)
                    : throw new SymbolException($"Symbol {name} is not defined in current scope");

        #endregion
    }
}
