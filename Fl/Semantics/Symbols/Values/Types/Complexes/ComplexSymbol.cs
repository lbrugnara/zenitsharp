// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Exceptions;
using Fl.Semantics.Types;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fl.Semantics.Symbols
{
    public abstract class ComplexSymbol : TypeSymbol, IComplexSymbol
    {
        /// <summary>
        /// Contains symbols defined in this scope
        /// </summary>
        private Dictionary<string, ISymbol> Symbols { get; }

        protected ComplexSymbol(string name, BuiltinType type, ISymbolContainer parent)
            : base(name, type, parent)
        {
            this.Symbols = new Dictionary<string, ISymbol>();
        }

        #region IComplexSymbol implementation

        public void Insert<T>(string name, T symbol)
            where T : ISymbol
        {
            if (this.Symbols.ContainsKey(name))
                throw new SymbolException($"Symbol {name} is already defined in current scope");

            this.Symbols[name] = symbol;
        }

        public void Remove(string name)
        {
            if (this.Symbols.ContainsKey(name))
                this.Symbols.Remove(name);
        }

        public bool Contains(string name)
        { 
            return this.Symbols.ContainsKey(name) || (this.Parent != null && this.Parent.Contains(name));
        }

        public T Get<T>(string name) 
            where T : ISymbol
        {
            var symbol = this.TryGet<T>(name);

            if (symbol == null)
                throw new SymbolException($"Symbol {name} is not defined in current scope");

            return symbol;
        }

        public T TryGet<T>(string name) 
            where T : ISymbol
        {
            if (this.Symbols.ContainsKey(name))
                return (T)this.Symbols[name];

            var symbol = this.Symbols.Values.OfType<T>().FirstOrDefault(v => v.Name == name);

            if (symbol != null)
                return (T)symbol;

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
            sb.AppendLine($"{nameIndent}[{this.BuiltinType} '{this.Name}']");

            foreach (var (name, symbolEntry) in this.Symbols.Where(kvp => kvp.Value is IBoundSymbol))
                sb.AppendLine($"{"".PadLeft(memberIndentN)}{name}: {(symbolEntry as IBoundSymbol).TypeSymbol.ToDebugString(memberIndentN)}");

            foreach (var (name, symbolEntry) in this.Symbols.Where(kvp => kvp.Value is ISymbolContainer))
                sb.AppendLine((symbolEntry as ISymbolContainer).ToDebugString(memberIndentN));

            return sb.ToString();
        }
    }
}
