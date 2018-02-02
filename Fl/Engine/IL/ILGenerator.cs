// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions;
using Fl.Parser.Ast;
using Fl.Engine.IL.Instructions.Operands;
using Fl.Engine.IL.VM;
using System.Collections.Generic;
using System.Linq;

namespace Fl.Engine.IL
{
    public class ILGenerator : IAstWalker<Operand>
    {
        /// <summary>
        /// Tracks variables per blocks
        /// </summary>
        public ILSymbolTable SymbolTable { get; }

        /// <summary>
        /// Contains program fragments and instructions
        /// </summary>
        public ILProgramBuilder Program { get; }

        /// <summary>
        /// Set of labels to be "resolved" on the next instructions emission
        /// </summary>
        private Stack<Label> labels { get; }

        /// <summary>
        /// All the visitor logic is moved to the AstVisitor
        /// </summary>
        private AstVisitor astVisitor;

        public ILGenerator()
        {
            this.SymbolTable = new ILSymbolTable();
            this.Program = new ILProgramBuilder();
            labels = new Stack<Label>();
            astVisitor = new AstVisitor(this);            
        }

        // Adds a new block to the SymbolTable, it represents a new scope
        public void EnterBlock(BlockType type, Label entryPoint = null, Label exitPoint = null)
        {
            this.SymbolTable.EnterBlock(type, entryPoint, exitPoint);
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
            this.Program.PushFragment(name, type);
            this.EnterBlock(BlockType.Common);
        }

        // Returns true if the current fragment is a function fragment
        public bool InFunction => Program.IsFunctionFragment();

        // Removes the current fragment and leaves the current symbol table block
        public void PopFragment()
        {
            this.LeaveBlock();
            this.Program.PopFragment();            
        }

        public void BindLabel(Label l)
        {
            labels.Push(l);
        }

        // Adds a new instruction to the current fragment
        public Instruction Emmit(Instruction i)
        {
            if (labels.Any())
                i.Label = labels.Pop();
            this.Program.CurrentFragment.AddInstruction(i);
            return i;
        }

        public ILProgram Build(AstNode node)
        {
            this.Visit(node);
            while (labels.Any())
                this.Emmit(new NopInstruction());
            return this.Program.Build();
        }

        public Operand Visit(AstNode node)
        {
            return astVisitor.Visit(node);
        }
    }
}
