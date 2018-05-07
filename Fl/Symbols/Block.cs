// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;
using System.Collections.Generic;
using Fl.Lang.Types;

namespace Fl.Symbols
{
    public class Block
    {
        /// <summary>
        /// Scope name used for mangled names
        /// </summary>
        public string Name { get; }

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

        public Block(BlockType type, string name)
        {
            this.Name = name;
            this.Type = type;
            this.Symbols = new Dictionary<string, Symbol>();
            this.Children = new Dictionary<string, Block>();
        }

        public Block(BlockType type, string name, Block global, Block parent = null)
            : this(type, name)
        {
            this.Global = global;
            this.Parent = parent;
        }

        public Block GetOrCreateChild(BlockType type, string name)
        {
            Block block = null;

            if (this.Children.ContainsKey(name))
            {
                block = this.Children[name];

                if (block.Type != type)
                    throw new Exception();
            }
            else
            {
                block = this.ChainBlock(type, name);
            }

            return block;
        }

        private Block ChainBlock(BlockType type, string name)
        {
            var block = new Block(type, name, this.Global, this);
            this.Children[name] = block;
            return block;
        }

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

            var symbol = new Symbol(name, type, this.Name);
            this.Symbols[name] = symbol;
            return symbol;
        }

        public bool HasSymbol(string name) => this.Symbols.ContainsKey(name) || (this.Parent != null && this.Parent.HasSymbol(name)) || (this.Global != null && this.Global.HasSymbol(name));

        public Symbol this[string name] =>
            this.Symbols.ContainsKey(name)
            ? this.Symbols[name]
            : this.Parent != null && this.Parent.HasSymbol(name)
                ? this.Parent?[name]
                : this.Global != null && this.Global.HasSymbol(name)
                    ? this.Global[name]
                    : throw new SymbolException($"Symbol {name} is not defined in current scope");
    }
}
