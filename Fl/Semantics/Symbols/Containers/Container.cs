
// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Semantics.Exceptions;
using Fl.Semantics.Symbols.Types;
using Fl.Semantics.Symbols.Values;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fl.Semantics.Symbols.Containers
{
    public abstract class Container : IContainer
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
        /// Contains symbols defined in this scope
        /// </summary>
        private Dictionary<string, ISymbol> TypeSymbols { get; }

        /// <summary>
        /// If present, reference to the parent scope
        /// </summary>
        public IContainer Parent { get; private set; }

        public Container(string name)
            : this (name, null)
        {            
        }

        public Container(string name, IContainer parent)
        {
            this.Name = name;
            this.Symbols = new Dictionary<string, ISymbol>();
            this.TypeSymbols = new Dictionary<string, ISymbol>();
            this.Parent = parent;
        }

        public override string ToString()
        {
            return $"container-{this.Name}";
        }
        
        private Dictionary<string, ISymbol> GetDestination<T>()
        {
            return typeof(ITypeSymbol).IsAssignableFrom(typeof(T)) ? this.TypeSymbols : this.Symbols;
        }        

        #region IBlock implementation

        public void Insert<T>(T symbol)
            where T : ISymbol
        {
            var destination = this.GetDestination<T>();

            if (destination.ContainsKey(symbol.Name))
                throw new SymbolException($"Symbol {symbol.Name} is already defined in current scope");

            destination[symbol.Name] = symbol;
        }

        public void Insert<T>(string name, T symbol)
            where T : ISymbol
        {
            var destination = this.GetDestination<T>();

            if (destination.ContainsKey(name))
                throw new SymbolException($"Symbol {name} is already defined in current scope");

            destination[name] = symbol;
        }

        public bool Contains<T>(string name) where T : ISymbol
        {
            var destination = this.GetDestination<T>();

            return destination.ContainsKey(name) && destination[name] is T || (this.Parent != null && this.Parent.Contains<T>(name));
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
            var destination = this.GetDestination<T>();

            if (destination.ContainsKey(name))
                return (T)destination[name];

            var symbol = destination.Values.OfType<T>().FirstOrDefault(v => v.Name == name);

            if (symbol != null)
                return (T)symbol;

            if (this.Parent == null || !this.Parent.Contains<T>(name))
                return default(T);

            return this.Parent.TryGet<T>(name);
        }

        public List<T> GetAllOfType<T>() where T : ISymbol
        {
            var all = this.Symbols.Values.OfType<T>().Cast<T>().ToList();

            all.AddRange(this.TypeSymbols.Values.OfType<T>().Cast<T>().ToList());

            return all;
        }

        #endregion

        public virtual string ToValueString()
        {
            return "";
        }

        public virtual string ToDumpString(int indent = 0)
        {
            int titleIndentN = indent + 2;
            int memberIndentN = indent + 4;

            // Title
            var nameIndent = "".PadLeft(indent);
            var memberIndent = "".PadLeft(memberIndentN);

            if (!this.Symbols.Any() && !this.TypeSymbols.Any())
                return $"{nameIndent}{this.Name} {{}}";

            var sb = new StringBuilder();
            sb.AppendLine($"{nameIndent}{this.Name} {{");

            // Variables
            if (this.Symbols.Any())
                sb.AppendLine($"{memberIndent}// Variables");

            foreach (var (name, symbol) in this.Symbols)
            {
                if (symbol is IBoundSymbol bs)
                {
                    sb.AppendLine($"{memberIndent}{bs.ToValueString()}");
                }
                else if (symbol is IContainer sc)
                {
                    sb.AppendLine(sc.ToDumpString(memberIndentN));
                }
                else
                {
                    sb.AppendLine($"NOT HANDLED SYMBOL {symbol.GetType()}");
                }
            }

            // Types
            if (this.TypeSymbols.Any())
                sb.AppendLine($"{memberIndent}// Types");

            foreach (var (name, symbol) in this.TypeSymbols)
            {
                if (symbol is IContainer sc)
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
