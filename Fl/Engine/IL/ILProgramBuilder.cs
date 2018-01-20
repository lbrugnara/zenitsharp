// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;
using Fl.Engine.IL.EValuators;

namespace Fl.Engine.IL
{
    public class ILProgramBuilder
    {
        private List<Fragment> _ResolvedFragments;
        private Stack<Fragment> _Fragments;

        public ILProgramBuilder()
        {
            var globalFragment = new Fragment(".global", FragmentType.Global);
            _ResolvedFragments = new List<Fragment>() { globalFragment };
            _Fragments = new Stack<Fragment>();
            _Fragments.Push(globalFragment);
        }

        public ILProgram Build() => new ILProgram(Symbols.SymbolTable.Instance, _ResolvedFragments);

        public Fragment CurrentFragment => _Fragments.Peek();

        public bool IsFunctionFragment() => CurrentFragment.Type == FragmentType.Function;

        public void PushFragment(string name, FragmentType type)
        {
            _Fragments.Push(new Fragment(name, type));
        }

        public void PopFragment()
        {
            _ResolvedFragments.Add(_Fragments.Pop());
        }

        public Label NewLabel(bool withDestination = false)
        {
            return new Label()
            {
                Address = withDestination ? CurrentFragment.NextAddress : -1
            };
        }

        public void BackpatchLabel(Label label)
        {
            if (label.Address != -1)
                throw new System.Exception($"Label already has a destination address: {label.Address}");

            label.Address = CurrentFragment.NextAddress;
        }
    }
}
