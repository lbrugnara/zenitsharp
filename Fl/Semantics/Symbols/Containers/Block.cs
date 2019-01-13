// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Semantics.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fl.Semantics.Symbols
{
    public class Block : ISymbolContainer
    {
        /// <summary>
        /// Symbol name (user-defined name)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Contains symbols defined in this scope
        /// </summary>
        private Dictionary<string, ISymbol> Symbols { get; }

        /// <summary>
        /// If present, reference to the parent scope
        /// </summary>
        public ISymbolContainer Parent { get; private set; }

        public Block(string name)
            : this (name, null)
        {            
        }

        public Block(string name, ISymbolContainer parent)
        {
            this.Name = name;
            this.Symbols = new Dictionary<string, ISymbol>();
            this.Parent = parent;
        }

        public override string ToString()
        {
            return $"block-{this.Name}";
        }        

        #region ISymbolContainer implementation

        public void Insert<T>(T symbol)
            where T : ISymbol
        {
            if (this.Symbols.ContainsKey(symbol.Name))
                throw new SymbolException($"Symbol {symbol.Name} is already defined in current scope");

            this.Symbols[symbol.Name] = symbol;
        }

        public void Remove(string name)
        {
            if (this.Symbols.ContainsKey(name))
                this.Symbols.Remove(name);
        }

        public void Insert<T>(string name, T symbol)
            where T : ISymbol
        {
            if (this.Symbols.ContainsKey(name))
                throw new SymbolException($"Symbol {name} is already defined in current scope");

            this.Symbols[name] = symbol;
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

        public virtual string ToDumpString(int indent = 0)
        {
            int titleIndentN = indent + 2;
            int memberIndentN = indent + 4;

            // Title
            var nameIndent = "".PadLeft(indent);

            if (!this.Symbols.Any())
                return $"{nameIndent}{this.Name} {{}}";

            var sb = new StringBuilder();
            sb.AppendLine($"{nameIndent}{this.Name} {{");

            foreach (var (name, symbol) in this.Symbols)
            {
                if (symbol is IBoundSymbol bs)
                {
                    sb.AppendLine($"{"".PadLeft(memberIndentN)}{bs.ToValueString()}");
                }
                else if (symbol is ISymbolContainer sc)
                {
                    sb.AppendLine(sc.ToDumpString(memberIndentN));
                }
                else
                {
                    sb.AppendLine($"NOT HANDLED SYMBOL {symbol.GetType()}");
                }
            }

            sb.Append($"{nameIndent}}}");

            return sb.ToString();
        }
    }
}
