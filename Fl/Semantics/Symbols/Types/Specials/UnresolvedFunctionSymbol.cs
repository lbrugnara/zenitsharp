// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Semantics.Symbols.Containers;
using Fl.Semantics.Types;

namespace Fl.Semantics.Symbols.Types.Specials
{
    public class UnresolvedFunctionSymbol : IUnresolvedTypeSymbol
    {
        public string Name { get; }

        public IContainer Parent { get; }

        public BuiltinType BuiltinType => BuiltinType.Function;

        public UnresolvedFunctionSymbol(string name, IContainer parent)
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
