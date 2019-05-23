// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Semantics.Symbols.Containers;
using Zenit.Semantics.Types;

namespace Zenit.Semantics.Symbols.Types.Primitives
{
    public class Primitive : IPrimitive
    {
        public BuiltinType BuiltinType { get; }

        public IContainer Parent { get; }

        public Primitive(BuiltinType type, IContainer parent)
        {
            this.BuiltinType = type;
            this.Parent = parent;
        }

        public string Name => this.BuiltinType.GetName();

        public override string ToString()
        {
            return this.BuiltinType.GetName();
        }

        public virtual string ToValueString() => this.ToString();
    }
}
