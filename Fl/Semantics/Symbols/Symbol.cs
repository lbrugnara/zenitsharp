// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

namespace Fl.Semantics.Symbols
{
    public class Symbol : ISymbol
    {
        /// <summary>
        /// Symbol name (user-defined name)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type information
        /// </summary>
        public TypeInfo TypeInfo { get; set; }

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
        public ISymbolContainer Parent { get; private set; }

        public Symbol(string name, TypeInfo type, Access access, Storage storage, ISymbolContainer parent)
        {
            this.Name = name;
            this.TypeInfo = type;
            this.Access = access;
            this.Storage = storage;
            this.Parent = parent;
        }

        public override string ToString()
        {
            var str = this.Access.ToKeyword();

            if (this.Storage != Storage.Immutable)
                str += $" {this.Storage.ToKeyword()}";

            str += $" {this.Name}: {this.TypeInfo}";

            return str;
        }

        public virtual string ToDebugString(int indent = 0)
        {
            return "".PadLeft(indent) + this.ToString();
        }
    }
}
