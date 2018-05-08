// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Lang.Types;
using System.Collections.Generic;

namespace Fl.Symbols
{
    public class Package : Symbol, ISymbolTable
    {
        /// <summary>
        /// Contains symbols defined in this block
        /// </summary>
        private Block Symbols { get; }

        public Package(string name, string scope = null)
            : base(name, Lang.Types.Package.Instance, scope)
        {
            this.Symbols = new Block(BlockType.Package, this.MangledName);
        }

        #region ISymbolTable implementation

        /// <inheritdoc/>
        public void AddSymbol(Symbol symbol) => this.Symbols.AddSymbol(symbol);

        /// <inheritdoc/>
        public Symbol NewSymbol(string name, Type type) => this.Symbols.NewSymbol(name, type);

        /// <inheritdoc/>
        public bool HasSymbol(string name) => this.Symbols.HasSymbol(name);

        /// <inheritdoc/>
        public Symbol GetSymbol(string name) => this.Symbols.GetSymbol(name);

        #endregion

        internal Package NewPackage(string name)
        {
            var pkg = new Package(name, this.MangledName);

            this.Symbols.AddSymbol(pkg);

            return pkg;
        }        
    }
}
