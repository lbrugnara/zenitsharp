// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Symbols.Types
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
        public Symbol NewSymbol(string name, Type type) => this.Scope.NewSymbol(name, type);

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
