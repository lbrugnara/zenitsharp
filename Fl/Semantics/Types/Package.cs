// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Symbols;

namespace Fl.Semantics.Types
{
    public class Package : Struct
    {
        /// <summary>
        /// Contains symbols defined in this package
        /// </summary>
        private Scope Scope { get; }

        public Package(string name, Scope global)
            : base(name)
        {
            this.Scope = new Scope(ScopeType.Package, name, global);
        }

        #region ISymbolTable implementation

        /// <inheritdoc/>
        public void AddSymbol(Symbol symbol) => this.Scope.AddSymbol(symbol);

        /// <inheritdoc/>
        public Symbol NewSymbol(string name, Type type, Access access, Storage storage) => this.Scope.NewSymbol(name, type, access, storage);

        /// <inheritdoc/>
        public bool HasSymbol(string name) => this.Scope.HasSymbol(name);

        /// <inheritdoc/>
        public Symbol GetSymbol(string name) => this.Scope.GetSymbol(name);

        #endregion

        internal Package NewPackage(string name)
        {
            var pkg = new Package(name, this.Scope.Global);

            //this.Scope.AddSymbol(pkg);

            return pkg;
        }
    }
}
