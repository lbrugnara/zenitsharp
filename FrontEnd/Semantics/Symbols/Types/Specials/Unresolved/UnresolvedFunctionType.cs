﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Zenit.Semantics.Symbols.Containers;
using Zenit.Semantics.Types;

namespace Zenit.Semantics.Symbols.Types.Specials.Unresolved
{
    public class UnresolvedFunctionType : IUnresolvedType
    {
        public string Name { get; }

        public IContainer Parent { get; }

        public BuiltinType BuiltinType => BuiltinType.Function;

        public UnresolvedFunctionType(string name, IContainer parent)
        {
            this.Name = name;
            this.Parent = parent;
        }
        
        public override string ToString()
        {
            return this.ToValueString();
        }

        public string ToValueString()
        {
            return $"unresolved func call {this.Name}";
        }
    }
}
