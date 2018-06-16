// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols.Types;

namespace Fl.Symbols
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
        public virtual Type Type { get; set; }


        public Symbol(string name, Type type)
        {
            this.Name = name;
            this.Type = type;
        }

        protected Symbol(string name)
        {
            this.Name = name;
        }

        public override string ToString()
        {
            return $"{this.Name}: {this.Type}";
        }
    }
}
