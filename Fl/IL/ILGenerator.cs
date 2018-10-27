// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.IL.Instructions;
using Fl.Ast;
using Fl.IL.Instructions.Operands;
using Fl.IL.VM;
using System.Collections.Generic;
using System.Linq;

namespace Fl.IL
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
        public ILProgramBuilder ProgramBuilder { get; }

        /// <summary>
        /// Set of labels to be "resolved" on the next instructions emission
        /// </summary>
        private Stack<Label> Labels { get; }

        /// <summary>
        /// All the visitor logic is moved to the AstVisitor
        /// </summary>
        private AstVisitor astVisitor;

        public ILGenerator()
        {
            this.SymbolTable = new ILSymbolTable();
            this.ProgramBuilder = new ILProgramBuilder();
            this.Labels = new Stack<Label>();
            this.astVisitor = new AstVisitor(this);            
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
            this.ProgramBuilder.PushFragment(name, type);
            this.EnterBlock(BlockType.Common);
        }

        // Returns true if the current fragment is a function fragment
        public bool InFunction => this.ProgramBuilder.IsFunctionFragment();

        // Removes the current fragment and leaves the current symbol table block
        public void PopFragment()
        {
            this.LeaveBlock();
            this.ProgramBuilder.PopFragment();            
        }

        public void BindLabel(Label l)
        {
            this.Labels.Push(l);
        }

        // Adds a new instruction to the current fragment
        public Instruction Emmit(Instruction i)
        {
            if (this.Labels.Any())
                i.Label = this.Labels.Pop();
            this.ProgramBuilder.CurrentFragment.AddInstruction(i);
            return i;
        }

        public ILProgram Build(Node node)
        {
            this.Visit(node);
            while (this.Labels.Any())
                this.Emmit(new NopInstruction());
            return this.ProgramBuilder.Build();
        }

        public Operand Visit(Node node)
        {
            return this.astVisitor.Visit(node);
        }
    }
}
