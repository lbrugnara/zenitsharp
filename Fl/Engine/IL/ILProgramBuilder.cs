// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;
using System.Linq;
using Fl.Engine.IL.Instructions;
using Fl.Engine.IL.VM;

namespace Fl.Engine.IL
{
    public class ILProgramBuilder
    {
        /// <summary>
        /// Built Fragments that are ready to be used in an ILProgram
        /// </summary>
        private List<Fragment> ResolvedFragments { get; }

        /// <summary>
        /// Fragments that currently being built
        /// </summary>
        private Stack<FragmentBuilder> Fragments { get; }

        /// <summary>
        /// Labels name are incremented with each NewLabel call
        /// </summary>
        private int labelCount = 1;

        public ILProgramBuilder()
        {
            this.ResolvedFragments = new List<Fragment>();
            this.Fragments = new Stack<FragmentBuilder>();

            // First fragment being built is the global one
            this.Fragments.Push(new FragmentBuilder(".global", FragmentType.Global));
        }

        /// <summary>
        /// Pending fragment (CurrentFragment) is the .global fragment. Build it and add it to the
        /// ResolvedFragments list to create an ILProgram
        /// </summary>
        /// <returns>A new ILProgram with the symbol table and the built fragments</returns>
        public ILProgram Build()
        {
            this.ResolvedFragments.Add(this.CurrentFragment.Build());
            return new ILProgram(Symbols.SymbolTable.Instance, this.ResolvedFragments);
        }

        /// <summary>
        /// Return the current fragment being built
        /// </summary>
        public FragmentBuilder CurrentFragment => this.Fragments.Peek();

        /// <summary>
        /// Return true if the CurrentFragment is a FragmentType.Function
        /// </summary>
        /// <returns></returns>
        public bool IsFunctionFragment() => this.CurrentFragment.Type == FragmentType.Function;

        /// <summary>
        /// Add a new FragmentBuilder object that will be the CurrentFragment one
        /// </summary>
        /// <param name="name">Fragment name</param>
        /// <param name="type">Fragment type</param>
        public void PushFragment(string name, FragmentType type)
        {
            this.Fragments.Push(new FragmentBuilder(name, type));
        }

        /// <summary>
        /// Finish the current FragmentBuilder by building it and adding it
        /// to the ResolvedFragments list
        /// </summary>
        public void PopFragment()
        {
            this.ResolvedFragments.Add(this.Fragments.Pop().Build());
        }

        /// <summary>
        /// Creates a new Label
        /// </summary>
        /// <returns></returns>
        public Label NewLabel() => new Label($"L{(this.labelCount++)}");
    }
}
