// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;

namespace Fl.Symbols
{

    public class Block : ISymbolTable
    {
        /// <summary>
        /// Scope unique id used in mangled names
        /// </summary>
        public string Uid { get; }

        /// <summary>
        /// Type of the current block
        /// </summary>
        public BlockType Type { get; }

        /// <summary>
        /// Contains symbols defined in this block
        /// </summary>
        Dictionary<string, Symbol> Symbols { get; }

        /// <summary>
        /// Reference to the Global block
        /// </summary>
        private Block Global { get; }

        /// <summary>
        /// If present, reference to the parent block
        /// </summary>
        private Block Parent { get; }

        /// <summary>
        /// List of block's children
        /// </summary>
        private Dictionary<string, Block> Children { get; }

        public Block(BlockType type, string uid)
        {
            this.Uid = uid;
            this.Type = type;
            this.Symbols = new Dictionary<string, Symbol>();
            this.Children = new Dictionary<string, Block>();

            if (type == BlockType.Function)
                this.NewSymbol("@ret", null);
        }

        public Block(BlockType type, string uid, Block global, Block parent = null)
            : this(type, uid)
        {
            this.Global = global;
            if (parent != global)
                this.Parent = parent;
        }

        public Block GetOrCreateInnerBlock(BlockType type, string uid)
        {
            Block block = null;

            if (this.Children.ContainsKey(uid))
            {
                block = this.Children[uid];

                if (block.Type != type)
                    throw new BlockException($"Expecting block {uid} to be of type {type} but it has type {block.Type}");
            }
            else
            {
                block = new Block(type, uid, this.Global, this);

                this.Children[uid] = block;
            }

            return block;
        }

        public bool IsFunction
        {
            get
            {
                return this.Type == BlockType.Function || Parent != null && Parent.IsFunction;
            }
        }

        #region ISymbolTable implementation

        public void AddSymbol(Symbol symbol)
        {
            if (this.Symbols.ContainsKey(symbol.Name))
                throw new SymbolException($"Symbol {symbol.Name} is already defined in current block");

            this.Symbols[symbol.Name] = symbol;
        }

        public Symbol NewSymbol(string name, Lang.Types.Type type)
        {
            if (this.Symbols.ContainsKey(name))
                throw new SymbolException($"Symbol {name} is already defined in current block");

            var symbol = new Symbol(name, type, this.Uid);
            this.Symbols[name] = symbol;
            return symbol;
        }

        public bool HasSymbol(string name) => this.Symbols.ContainsKey(name) || (this.Parent != null && this.Parent.HasSymbol(name)) || (this.Global != null && this.Global.HasSymbol(name));

        public Symbol GetSymbol(string name) =>
            this.Symbols.ContainsKey(name)
            ? this.Symbols[name]
            : this.Parent != null && this.Parent.HasSymbol(name)
                ? this.Parent.GetSymbol(name)
                : this.Global != null && this.Global.HasSymbol(name)
                    ? this.Global.GetSymbol(name)
                    : throw new SymbolException($"Symbol {name} is not defined in current scope");

        #endregion
    }
}
