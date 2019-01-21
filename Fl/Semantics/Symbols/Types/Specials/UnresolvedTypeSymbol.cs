// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Semantics.Symbols.Containers;
using Fl.Semantics.Types;

namespace Fl.Semantics.Symbols.Types.Specials
{
    public class UnresolvedExpressionType : IUnresolvedTypeSymbol
    {
        public BuiltinType BuiltinType => BuiltinType.None;

        public string Name { get; }

        public IContainer Parent { get; }

        public ITypeSymbol Left { get; }

        public ITypeSymbol Right { get; }

        public UnresolvedExpressionType(string name, IContainer parent, ITypeSymbol left, ITypeSymbol right)
        {
            this.Name = name;
            this.Parent = parent;
            this.Left = left;
            this.Right = right;
        }

        public string ToValueString()
        {
            return "unresolved expression type";
        }
    }

    public class UnresolvedTypeSymbol : IUnresolvedTypeSymbol
    {
        public string Name { get; }

        public IContainer Parent { get; }

        public BuiltinType BuiltinType { get; set; }

        public UnresolvedTypeSymbol(string name, IContainer parent)
        {
            this.Name = name;
            this.Parent = parent;
            this.BuiltinType = BuiltinType.None;
        }

        public override string ToString()
        {
            return this.ToValueString();
        }

        public string ToValueString()
        {
            return $"unresolved {this.Name}";
        }
    }
}
