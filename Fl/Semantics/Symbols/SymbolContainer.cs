// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Exceptions;
using Fl.Semantics.Types;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fl.Semantics.Symbols
{
    public abstract class SymbolContainer : Symbol, ISymbolContainer
    {
        /// <summary>
        /// Contains symbols defined in this scope
        /// </summary>
        private Dictionary<string, Symbol> Symbols { get; }

        /// <summary>
        /// If present, reference to the parent scope
        /// </summary>
        protected SymbolContainer Parent { get; set; }

        /// <summary>
        /// This constructor creates an instance that represents the Global scope, as caller
        /// is not providing a prent scope
        /// </summary>
        /// <param name="name">Scope's UID</param>
        public SymbolContainer(string name)
            : base(name)
        {
            this.Symbols = new Dictionary<string, Symbol>();
        }

        /// <summary>
        /// If the parent scope is present, caller is creating a Common scope.
        /// We navigate through the chain to get a reference to the global scope
        /// </summary>
        /// <param name="name">Scope's UID</param>
        /// <param name="parent">Parent scope of the new instance</param>
        public SymbolContainer(string name, SymbolContainer parent)
            : this(name)
        {
            this.Parent = parent;
        }

        public void AddChild(SymbolContainer child)
        {
            this.Symbols[child.Name] = child;
        }

        public bool HasChild(string name) => this.Symbols.ContainsKey(name);

        public SymbolContainer GetChild(string name) => this.Symbols[name] as SymbolContainer;

        #region ISymbolContainer implementation

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

        public override string ToDebugString(int indent = 0)
        {
            int titleIndentN = indent + 1;
            int memberIndentN = indent + 2;

            var sb = new StringBuilder();

            // Title
            var nameIndent = "".PadLeft(indent);
            sb.AppendLine($"{nameIndent}[{this.GetType().Name.Replace("SymbolContainer", "")} '{this.Name}']");

            // Symbols
            /*var memberIndent = "".PadLeft(titleIndentN);
            sb.AppendLine($"{memberIndent}[Symbols]");*/

            foreach (var symbol in this.GetAllSymbols())
                sb.AppendLine($"{symbol.ToDebugString(memberIndentN)}");

            return sb.ToString();
        }
    }
}
