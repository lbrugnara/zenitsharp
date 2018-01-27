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
        private List<FragmentBuilder> _ResolvedFragments;
        private Stack<FragmentBuilder> _Fragments;

        private int _labelCount = 1;

        public ILProgramBuilder()
        {
            var globalFragment = new FragmentBuilder(".global", FragmentType.Global);
            _ResolvedFragments = new List<FragmentBuilder>() { globalFragment };
            _Fragments = new Stack<FragmentBuilder>();
            _Fragments.Push(globalFragment);
        }

        public ILProgram Build() => new ILProgram(Symbols.SymbolTable.Instance, _ResolvedFragments.Select(rf => rf.Build()).ToList());

        public FragmentBuilder CurrentFragment => _Fragments.Peek();

        public bool IsFunctionFragment() => CurrentFragment.Type == FragmentType.Function;

        public void PushFragment(string name, FragmentType type)
        {
            _Fragments.Push(new FragmentBuilder(name, type));
        }

        public void PopFragment()
        {
            _ResolvedFragments.Add(_Fragments.Pop());
        }

        public Label NewLabel()
        {
            return new Label($"L{(_labelCount++)}");
        }
        /*
        public void BackpatchLabel(Label label)
        {
            if (label.Address != -1)
                throw new System.Exception($"Label already has a destination address: {label.Address}");

            label.Address = CurrentFragment.NextAddress;
        }*/
    }
}
