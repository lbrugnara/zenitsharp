// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Semantics.Types;

namespace Fl.Semantics.Symbols
{
    public class UnresolvedTypeSymbol : IUnresolvedTypeSymbol
    {
        public string Name { get; }

        public ISymbolContainer Parent { get; }

        public BuiltinType BuiltinType { get; set; }

        public UnresolvedTypeSymbol(string name, ISymbolContainer parent)
        {
            this.Name = name;
            this.Parent = parent;
            this.BuiltinType = BuiltinType.None;
        }

        public bool IsFunction
        {
            get => this.BuiltinType == BuiltinType.Function;
            set => this.BuiltinType = BuiltinType.Function;
        }

        public override string ToString()
        {
            return this.ToValueString();
        }

        public string ToValueString()
        {
            if (this.IsFunction)
                return $"unresolved func call {this.Name}";

            return $"unresolved {this.Name}";
        }
    }
}
