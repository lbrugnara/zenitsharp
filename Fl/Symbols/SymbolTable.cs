// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;
using Fl.Engine.Symbols.Types;
using Fl.Engine.Symbols.Exceptions;
using System.Linq;
using Fl.Lang.Types;

namespace Fl.Symbols
{
    public class SymbolTable
    {
        private Stack<Block> Blocks { get; }

        private int tempVarCounter = 0;

        public SymbolTable()
        {
            this.Blocks = new Stack<Block>();
            this.Blocks.Push(new Block(BlockType.Global, "0"));
        }

        public Block CurrentBlock => this.Blocks.Peek();

        public bool SymbolIsDefinedInBlock(string name) => this.CurrentBlock.HasSymbol(name);

        public void AddSymbol(Symbol symbol)
        {
            this.CurrentBlock.AddSymbol(symbol);
        }

        public Symbol NewSymbol(string name, Type type, bool resolved = true)
        {
            return this.CurrentBlock.NewSymbol(name, type);            
        }

        public Symbol NewTempSymbol(Type type, string suggestedName = null) 
            => this.NewSymbol($"@{suggestedName ?? "t"}{(this.tempVarCounter++)}", type);

        public Symbol GetSymbol(string name)
        {
            return this.CurrentBlock[name];
        }

        public void EnterBlock(BlockType blockType, string name)
        {
            var block = this.CurrentBlock.GetOrCreateChild(blockType, name);
            this.Blocks.Push(block);
        }
        
        public void LeaveBlock()
        {
            this.Blocks.Pop();
        }
        
        public Block GetLoopBlock()
        {
            /*if (!this.InBlock)
                throw new ScopeOperationException("Current block is not a loop");

            for (int i = this.blocks.Count-1; i >= 0; i--)
            {
                var block = this.blocks.ElementAt(i);
                if (block.Type == BlockType.Loop)
                    return block;
            }*/

            throw new ScopeOperationException("Current block is not a loop");
        }
    }
}
