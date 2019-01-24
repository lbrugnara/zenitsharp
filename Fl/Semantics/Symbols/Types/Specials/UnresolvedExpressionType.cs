// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Semantics.Symbols.Containers;
using Fl.Semantics.Types;
using System.Collections.Generic;

namespace Fl.Semantics.Symbols.Types.Specials
{
    public class UnresolvedTupleType : IUnresolvedTypeSymbol
    {
        public BuiltinType BuiltinType => BuiltinType.None;

        public string Name { get; }

        public IContainer Parent { get; }

        public List<ITypeSymbol> Types { get; }
        
        public UnresolvedTupleType(IContainer parent, List<ITypeSymbol> types)
        {
            this.Name = $"unresolved tuple type ({string.Join(',', types)})";
            this.Parent = parent;
            this.Types = types;
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

    public class UnresolvedExpressionType : IUnresolvedTypeSymbol
    {
        public BuiltinType BuiltinType => BuiltinType.None;

        public string Name { get; }

        public IContainer Parent { get; }

        public ITypeSymbol Left { get; }

        public ITypeSymbol Right { get; }

        public UnresolvedExpressionType(IContainer parent, ITypeSymbol left, ITypeSymbol right)
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
