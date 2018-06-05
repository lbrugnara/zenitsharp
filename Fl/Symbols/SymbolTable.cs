// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;
using Fl.Engine.Symbols.Types;
using Fl.Engine.Symbols.Exceptions;
using System.Linq;
using Fl.Lang.Types;

namespace Fl.Symbols
{
    public class SymbolTable : ISymbolTable
    {
        /// <summary>
        /// An stack to keep track of nested blocks
        /// </summary>
        private Stack<Block> Blocks { get; }

        /// <summary>
        /// Used in temporal symbol's name
        /// </summary>
        private int tempVarCounter = 0;

        public SymbolTable()
        {
            this.Blocks = new Stack<Block>();

            // Push the initial block (Global)
            this.Blocks.Push(new Block(BlockType.Global, "0"));
        }

        /// <summary>
        /// Current block is the last one added to the stack
        /// </summary>
        public Block CurrentBlock => this.Blocks.Peek();

        /// <summary>
        /// Check if there's a child block in CurrentBlock with the provided UID. If the block exists, check the block's type
        /// and if it differs throw an exception because block definition mismatches.
        /// If block does not exist, create a new block and chain it to the current block.
        /// Either way, push retrieved/created block to the stack to make it the CurrentBlock.
        /// </summary>
        /// <param name="blockType">Type of the block to get/create</param>
        /// <param name="uid">ID of the block to get/create</param>
        public void EnterBlock(BlockType blockType, string uid) => this.Blocks.Push(this.CurrentBlock.GetOrCreateInnerBlock(blockType, uid));

        public void EnterFunctionBlock(Function functionBlock) => this.Blocks.Push(functionBlock.Block);

        /// <summary>
        /// Remove CurrentBlock from the stack (go back to the CurrentBlock's parent block)
        /// </summary>
        public void LeaveBlock() => this.Blocks.Pop();


        #region ISymbolTable implementation

        /// <inheritdoc/>
        public void AddSymbol(Symbol symbol) => this.CurrentBlock.AddSymbol(symbol);

        /// <inheritdoc/>
        public Symbol NewSymbol(string name, Type type) => this.CurrentBlock.NewSymbol(name, type);

        /// <inheritdoc/>
        public bool HasSymbol(string name) => this.CurrentBlock.HasSymbol(name);

        /// <inheritdoc/>
        public Symbol GetSymbol(string name) => this.CurrentBlock.GetSymbol(name);

        #endregion

        /// <summary>
        /// Create a temporal symbol and add it to the CurrentBlock
        /// </summary>
        /// <param name="type">Temp symbol's type</param>
        /// <param name="suggestedName">Temp symbol's name can be specified here</param>
        /// <returns></returns>
        public Symbol NewTempSymbol(Type type, string suggestedName = null) 
            => this.NewSymbol($"@{suggestedName ?? "t"}{(this.tempVarCounter++)}", type);
    }
}
