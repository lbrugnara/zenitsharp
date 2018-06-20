// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;
using Fl.IL.Instructions.Operands;
using Fl.Engine.Symbols.Types;
using Fl.Semantics.Exceptions;
using System.Linq;
using Fl.IL.Instructions;

namespace Fl.IL
{
    public class ILSymbolTable
    {
        // Represents the global block (main block)
        private readonly Block global;
        
        // List of the current chain
        private readonly List<Block> blocks;
        
        // Once a block is finished it is moved to this list
        private readonly List<Block> resolvedBlocks;

        public SymbolOperand ReturnSymbol { get; } = new SymbolOperand("@return", OperandType.Auto);

        private int tempVarCounter = 0;

        private int blockCounter = 1;

        public ILSymbolTable()
        {
            this.global = new Block(BlockType.Global, "0", null, null);
            this.blocks = new List<Block>();
            this.resolvedBlocks = new List<Block>();            
        }

        // The symbol table is inside a block if there is at least one entry in the _Blocks list
        private bool InBlock => this.blocks.Count() > 0;

        // The current block is the last in the chain or the global block
        public Block CurrentBlock => this.InBlock ? this.blocks.Last() : this.global;

        public bool SymbolIsDefinedInBlock(string name)
            => this.InBlock ? this.CurrentBlock.HasSymbol(name) : this.global.HasSymbol(name);

        public SymbolOperand NewSymbol(string name, OperandType type)
        {
            var symbolOperand = new SymbolOperand(name, type, this.CurrentBlock.Name);
            this.CurrentBlock[name] = symbolOperand;
            return symbolOperand;
        }

        public SymbolOperand NewTempSymbol(OperandType type, string suggestedName = null) 
            => this.NewSymbol($"@{suggestedName ?? "t"}{(this.tempVarCounter++)}", type);

        public SymbolOperand GetSymbol(string name)
        {
            if (this.InBlock)
            {
                for (int i = this.blocks.Count-1; i >= 0; i--)
                {
                    var scope = this.blocks[i];
                    if (scope.HasSymbol(name))
                        return scope[name];
                }
            }

            if (this.global != null && this.global.HasSymbol(name))
                return global[name];

            return null;
        }

        public void EnterBlock(BlockType blockType, Label entryPoint, Label exitPoint)
        {
            this.blocks.Add(new Block(blockType, this.blockCounter++.ToString(), entryPoint, exitPoint));
        }
        
        public void LeaveBlock()
        {
            var popblock = this.blocks.ElementAt(this.blocks.Count - 1);
            this.blocks.Remove(popblock);
            this.resolvedBlocks.Add(popblock);
        }
        
        public Block GetLoopBlock()
        {
            if (!this.InBlock)
                throw new ScopeOperationException("Current block is not a loop");

            for (int i = this.blocks.Count-1; i >= 0; i--)
            {
                var block = this.blocks.ElementAt(i);
                if (block.Type == BlockType.Loop)
                    return block;
            }

            throw new ScopeOperationException("Current block is not a loop");
        }
    }
}
