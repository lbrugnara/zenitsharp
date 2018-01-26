// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions;
using Fl.Parser.Ast;
using Fl.Engine.IL.Instructions.Operands;
using Fl.Engine.IL.VM;

namespace Fl.Engine.IL
{
    public class ILGenerator : IAstWalker<Operand>
    {
        // Tracks variables per blocks
        private ILSymbolTable _IlSymbolTable;

        // Contains program fragments and instructions
        private ILProgramBuilder _IlProgramBuilder;

        // All the visitor logic is moved to the AstVisitor
        private AstVisitor _AstVisitor;

        public ILGenerator()
        {
            _IlSymbolTable = new ILSymbolTable();
            _IlProgramBuilder = new ILProgramBuilder();
            _AstVisitor = new AstVisitor(this);
        }

        // Reference to the SymbolTable
        public ILSymbolTable SymbolTable => _IlSymbolTable;

        // Reference to the program being built
        public ILProgramBuilder Program => _IlProgramBuilder;

        // Adds a new block to the SymbolTable, it represents a new scope
        public void EnterBlock(BlockType type, Label entryPoint = null, Label exitPoint = null)
        {
            this._IlSymbolTable.EnterBlock(type, entryPoint, exitPoint);
        }

        // Leave the current block in the SymbolTable
        public void LeaveBlock()
        {
            this.SymbolTable.LeaveBlock();
        }

        // Adds a new fragment to the program, like a function fragment. It also adds a new block
        // in the symbol table
        public void PushFragment(string name, FragmentType type)
        {
            _IlProgramBuilder.PushFragment(name, type);
            EnterBlock(BlockType.Common);
        }

        // Returns true if the current fragment is a function fragment
        public bool InFunction => _IlProgramBuilder.IsFunctionFragment();

        // Removes the current fragment and leaves the current symbol table block
        public void PopFragment()
        {
            LeaveBlock();
            _IlProgramBuilder.PopFragment();            
        }

        // Adds a new instruction to the current fragment
        public Instruction Emmit(Instruction i)
        {
            _IlProgramBuilder.CurrentFragment.AddInstruction(i);
            return i;
        }

        public Operand Visit(AstNode node)
        {
            return _AstVisitor.Visit(node);
        }
    }
}
