// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Lang.Types;

namespace Fl.Symbols
{
    public class Function : Symbol, ISymbolTable
    {
        /// <summary>
        /// Contains symbols defined in this block
        /// </summary>
        public Block Block { get; private set; }

        public string[] Parameters { get; private set; }

        public Function(string name, string[] parameters, string scope = null)
            : base(name, new Func(), scope)
        {
            this.Block = new Block(BlockType.Function, this.MangledName);
            this.Parameters = parameters;
        }

        #region ISymbolTable implementation

        /// <inheritdoc/>
        public void AddSymbol(Symbol symbol) => this.Block.AddSymbol(symbol);

        /// <inheritdoc/>
        public Symbol NewSymbol(string name, Type type) => this.Block.NewSymbol(name, type);

        /// <inheritdoc/>
        public bool HasSymbol(string name) => this.Block.HasSymbol(name);

        /// <inheritdoc/>
        public Symbol GetSymbol(string name) => this.Block.GetSymbol(name);

        #endregion
    }
}
