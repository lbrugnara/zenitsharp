// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Semantics.Symbols
{
    public class Symbol
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
        public Access Access { get; set; }

        /// <summary>
        /// Symbol's storage type
        /// </summary>
        public Storage Storage { get; set; }

        public Symbol(string name, TypeInfo type, Access access, Storage storage)
        {
            this.Name = name;
            this.TypeInfo = type;
            this.Access = access;
            this.Storage = storage;
        }

        protected Symbol(string name)
        {
            this.Name = name;
        }

        public override string ToString()
        {
            return $"{this.Access.ToKeyword()} {this.Storage.ToKeyword()} {this.Name}: {this.TypeInfo}";
        }
    }
}
