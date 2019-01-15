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
        protected Dictionary<string, ISymbol> Symbols { get; }

        protected ComplexSymbol(string name, BuiltinType type, ISymbolContainer parent)
            : base(name, type, parent)
        {
            this.Symbols = new Dictionary<string, ISymbol>();
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
                return false;

            var objectType = obj as ComplexSymbol;

            if (objectType == null)
                return false;

            var objectSymbols = objectType.Symbols.Values.Where(s => s is IBoundSymbol).ToList();

            foreach (var member in objectSymbols)
            {
                if (!this.Symbols.ContainsKey(member.Name) || this.Symbols[member.Name] != member)
                    return false;
            }

            return true;
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

        public List<T> GetSymbols<T>() where T : ISymbol => this.Symbols.Values.OfType<T>().Cast<T>().ToList();

        #endregion

        public abstract string ToSafeString(params (ITypeSymbol type, string safestr)[] safeTypes);

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
