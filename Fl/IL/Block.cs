// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;
using Fl.IL.Instructions;
using Fl.IL.Instructions.Operands;

namespace Fl.IL
{

    public class Block
    {
        /// <summary>
        /// Scope name used for mangled names
        /// </summary>
        public string Name { get; }

        public BlockType Type { get; }

        /// <summary>
        /// Contains symbols defined in this block
        /// </summary>
        Dictionary<string, SymbolOperand> Symbols { get; }

        /// <summary>
        /// Label that represents the block's entry point. Used by
        /// continue statement
        /// </summary>
        public Label EntryPoint { get; }

        /// <summary>
        /// Label that represents the block's exit point. Used by
        /// break statement
        /// </summary>
        public Label ExitPoint { get; }

        public Block(BlockType type, string name, Label entryPoint, Label exitPoint)
        {
            this.Name = name;
            this.Type = type;
            this.Symbols = new Dictionary<string, SymbolOperand>();            
            this.EntryPoint = entryPoint;
            this.ExitPoint = exitPoint;
        }

        public bool HasSymbol(string name) => Symbols.ContainsKey(name);

        public SymbolOperand this[string name]
        {
            get
            {
                return Symbols[name];
            }
            set
            {
                Symbols[name] = value;
            }
        }
    }
}
