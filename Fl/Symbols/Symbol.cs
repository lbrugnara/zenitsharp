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
        public virtual SType Type { get; set; }

        /// <summary>
        /// Scope name
        /// </summary>
        public string ScopeName { get; private set; }

        /// <summary>
        /// FQN used by the compiler
        /// </summary>
        public string MangledName => ScopeName != null ? $"{this.ScopeName}__{this.Name}" : this.Name;


        public Symbol(string name, SType type, string scope = null)
        {
            this.Name = name;
            this.Type = type;
            this.ScopeName = scope;
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
