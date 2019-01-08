// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Semantics.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fl.Semantics.Symbols
{
    public class ComplexSymbol : Symbol, IComplexSymbol
    {
        /// <summary>
        /// Contains symbols defined in this scope
        /// </summary>
        private Dictionary<string, ISymbolTableEntry> Symbols { get; }

        public ComplexSymbol(string name, TypeInfo type, Access access, Storage storage, ISymbolContainer parent)
            : base (name, type, access, storage, parent)
        {
            this.Symbols = new Dictionary<string, ISymbolTableEntry>();
        }

        protected ComplexSymbol(string name, ISymbolContainer parent)
            : this (name, null, Access.Public, Storage.Immutable, parent)
        {
        }

        public List<ISymbolTableEntry> GetAllSymbols() => this.Symbols.Values.ToList();

        #region ISymbolContainer implementation

        public void Add<T>(T symbol)
            where T : ISymbolTableEntry
        {
            if (this.Symbols.ContainsKey(symbol.Name))
                throw new SymbolException($"Symbol {symbol.Name} is already defined in current scope");

            this.Symbols[symbol.Name] = symbol;
        }

        public bool Contains(string name)
        { 
            return this.Symbols.ContainsKey(name) || (this.Parent != null && this.Parent.Contains(name));
        }

        public T Get<T>(string name) 
            where T : ISymbolTableEntry
        {
            var symbol = this.TryGet<T>(name);

            if (symbol == null)
                throw new SymbolException($"Symbol {name} is not defined in current scope");

            return symbol;
        }

        public T TryGet<T>(string name) 
            where T : ISymbolTableEntry
        {
            if (this.Symbols.ContainsKey(name))
                return (T)this.Symbols[name];

            if (this.Parent == null || !this.Parent.Contains(name))
                return default(T);

            return this.Parent.TryGet<T>(name);
        }

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
