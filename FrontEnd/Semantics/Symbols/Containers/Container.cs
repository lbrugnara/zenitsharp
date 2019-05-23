
// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Zenit.Semantics.Exceptions;
using Zenit.Semantics.Symbols.Types;
using Zenit.Semantics.Symbols.Variables;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zenit.Semantics.Symbols.Containers
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
        protected Dictionary<string, ISymbol> Symbols { get; }

        /// <summary>
        /// Contains blocks defined in this scope
        /// </summary>
        protected Dictionary<string, ISymbol> Blocks { get; }

        /// <summary>
        /// Contains type symbols defined in this scope
        /// </summary>
        protected Dictionary<string, ISymbol> TypeSymbols { get; }

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
            this.Blocks = new Dictionary<string, ISymbol>();
            this.TypeSymbols = new Dictionary<string, ISymbol>();
            this.Parent = parent;
        }

        public override string ToString()
        {
            return $"container-{this.Name}";
        }
        
        private Dictionary<string, ISymbol> GetDestination<T>()
        {
            if (typeof(IType).IsAssignableFrom(typeof(T)))
                return this.TypeSymbols;

            if (typeof(IContainer).IsAssignableFrom(typeof(T)))
                return this.Blocks;

            return this.Symbols;
        }        

        #region IBlock implementation

        public virtual void Insert<T>(T symbol)
            where T : ISymbol
        {
            var destination = this.GetDestination<T>();

            if (destination.ContainsKey(symbol.Name))
                throw new SymbolException($"Symbol {symbol.Name} is already defined in current scope");

            destination[symbol.Name] = symbol;
        }

        public virtual void Insert<T>(string name, T symbol)
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
            all.AddRange(this.Blocks.Values.OfType<T>().Cast<T>().ToList());

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

            if (!this.Symbols.Any() && !this.Blocks.Any() && !this.TypeSymbols.Any())
                return $"{nameIndent}{this.Name} {{}}";

            var sb = new StringBuilder();
            sb.AppendLine($"{nameIndent}{this.Name} {{");

            // Variables
            if (this.Symbols.Any())
                sb.AppendLine(this.DumpTable(this.Symbols, "Variables", memberIndentN));

            // Types
            if (this.TypeSymbols.Any())
                sb.AppendLine(this.DumpTable(this.TypeSymbols, "Types", memberIndentN));

            // Blocks
            if (this.Blocks.Any())
                sb.AppendLine(this.DumpTable(this.Blocks, "Blocks", memberIndentN));            

            sb.Append($"{nameIndent}}}");

            return sb.ToString();
        }

        private string DumpTable(Dictionary<string, ISymbol> symbols, string title, int memberIndentN)
        {
            var memberIndent = "".PadLeft(memberIndentN);

            var sb = new StringBuilder();

            if (symbols.Any())
                sb.AppendLine($"{memberIndent}// {title}");

            foreach (var (name, symbol) in symbols)
            {
                if (symbol is IVariable bs)
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

            return sb.ToString();
        }
    }
}
