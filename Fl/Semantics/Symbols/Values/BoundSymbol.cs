// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Symbols.Containers;
using Fl.Semantics.Symbols.Types;

namespace Fl.Semantics.Symbols.Values
{
    public class BoundSymbol : IBoundSymbol
    {
        /// <summary>
        /// Symbol name (user-defined name)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type information
        /// </summary>
        public ITypeSymbol TypeSymbol { get; set; }

        /// <summary>
        /// Symbol's access level
        /// </summary>
        public virtual Access Access { get; private set; }

        /// <summary>
        /// Symbol's storage type
        /// </summary>
        public Storage Storage { get; private set; }

        /// <summary>
        /// If present, reference to the parent scope
        /// </summary>
        public IContainer Parent { get; private set; }

        public BoundSymbol(string id, ITypeSymbol typeSymbol, Access access, Storage storage, IContainer parent)
        {
            this.Name = id;
            this.TypeSymbol = typeSymbol;
            this.Access = access;
            this.Storage = storage;
            this.Parent = parent;
        }

        public void ChangeType(ITypeSymbol type)
        {
            this.TypeSymbol = type;
        }

        public override string ToString()
        {
            var str = this.Access.ToKeyword();

            if (this.Storage != Storage.Immutable)
                str += $" {this.Storage.ToKeyword()}";

            str += $" {this.Name}: {this.TypeSymbol}";

            return str;
        }

        public string ToValueString()
        {
            return $"{this.Name}: {this.TypeSymbol.ToValueString()}";
        }
    }
}
