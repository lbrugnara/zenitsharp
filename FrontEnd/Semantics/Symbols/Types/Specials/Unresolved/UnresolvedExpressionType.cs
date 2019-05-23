// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Semantics.Symbols.Containers;
using Zenit.Semantics.Types;

namespace Zenit.Semantics.Symbols.Types.Specials.Unresolved
{

    public class UnresolvedExpressionType : IUnresolvedType
    {
        public BuiltinType BuiltinType => BuiltinType.None;

        public string Name { get; }

        public IContainer Parent { get; }

        public IType Left { get; }

        public IType Right { get; }

        public UnresolvedExpressionType(IContainer parent, IType left, IType right)
        {
            this.Name = $"unresolved expression type ({left}, {right})";
            this.Parent = parent;
            this.Left = left;
            this.Right = right;
        }

        public string ToValueString()
        {
            return this.Name;
        }

        public override string ToString()
        {
            return this.ToValueString();
        }
    }
}
