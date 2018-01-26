// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;
using Fl.Engine.IL.Instructions.Operands;
using Fl.Engine.Symbols.Types;
using Fl.Engine.Symbols.Exceptions;
using System.Linq;

namespace Fl.Engine.IL
{
    public class ILSymbolTable
    {
        // Represents the global block (main block)
        private Block _Global;
        
        // List of the current chain
        private List<Block> _Blocks;
        
        // Once a block is finished it is moved to this list
        private List<Block> _ResolvedBlocks;


        private int TemporalVarCounter = 0;

        private int BlockCounter = 1;

        public ILSymbolTable()
        {
            _Global = new Block(BlockType.Global, "0", null, null);
            _Blocks = new List<Block>();
            _ResolvedBlocks = new List<Block>();            
        }

        // The symbol table is inside a block if there is at least one entry in the _Blocks list
        private bool InBlock => _Blocks.Count() > 0;

        // The current block is the last in the chain or the global block
        private Block CurrentBlock => InBlock ? _Blocks.Last() : _Global;

        public bool SymbolIsDefinedInBlock(string name)
        {
            if (InBlock)
                return CurrentBlock.HasSymbol(name);
            return _Global.HasSymbol(name);
        }

        public SymbolOperand NewSymbol(string name, TypeResolver typeResolver = null)
        {
            var symbol = new SymbolOperand(name, CurrentBlock.Name, typeResolver);
            CurrentBlock[name] = symbol;
            return symbol;
        }

        public SymbolOperand NewTempSymbol(string suggestedName = null) => NewSymbol($"@{suggestedName ?? "t"}{(TemporalVarCounter++)}");

        public SymbolOperand GetSymbol(string name)
        {
            if (InBlock)
            {
                for (int i = _Blocks.Count-1; i >= 0; i--)
                {
                    var scope = _Blocks[i];
                    if (scope.HasSymbol(name))
                        return scope[name];
                }
            }
            if (_Global != null && _Global.HasSymbol(name))
                return _Global[name];

            return new SymbolOperand(name, null);
        }

        public void EnterBlock(BlockType blockType, Label entryPoint, Label exitPoint)
        {
            _Blocks.Add(new Block(blockType, BlockCounter++.ToString(), entryPoint, exitPoint));
        }
        
        public void LeaveBlock()
        {
            var popblock = _Blocks.ElementAt(_Blocks.Count - 1);
            _Blocks.Remove(popblock);
            _ResolvedBlocks.Add(popblock);
        }
        
        public Block GetLoopBlock()
        {
            if (!InBlock)
                throw new ScopeOperationException("Current block is not a loop");

            for (int i = _Blocks.Count-1; i >= 0; i--)
            {
                var block = _Blocks.ElementAt(i);
                if (block.Type == BlockType.Loop)
                    return block;
            }
            throw new ScopeOperationException("Current block is not a loop");
        }
    }
}
