// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Lang.Types;
using System.Collections.Generic;

namespace Fl.Symbols
{
    public class Package : Symbol
    {
        /// <summary>
        /// Contains symbols defined in this block
        /// </summary>
        private Dictionary<string, Symbol> Symbols { get; }

        public Package(string name, string scope = null)
            : base(name, Lang.Types.Package.Instance, scope)
        {
            this.Symbols = new Dictionary<string, Symbol>();
        }

        public void AddSymbol(Symbol symbol)
        {
            if (this.Symbols.ContainsKey(symbol.Name))
                throw new SymbolException($"Symbol {symbol.Name} is already defined in package {this.Name}");

            this.Symbols[symbol.Name] = symbol;
        }

        internal Package NewPackage(string name)
        {
            if (this.Symbols.ContainsKey(name))
                throw new SymbolException($"Symbol {name} is already defined in package {this.Name}");

            var pkg = new Package(name, this.MangledName);
            this.Symbols[name] = pkg;
            return pkg;
        }

        public Symbol NewSymbol(string name, Type type)
        {
            if (this.Symbols.ContainsKey(name))
                throw new SymbolException($"Symbol {name} is already defined in package {this.Name}");

            var symbol = new Symbol(name, type, this.MangledName);
            this.Symbols[name] = symbol;
            return symbol;
        }

        public bool HasSymbol(string name) => this.Symbols.ContainsKey(name);

        public Symbol this[string name] => this.Symbols[name];
    }
}
