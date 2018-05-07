// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Lang.Types;

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
        public Type Type { get; set; }

        /// <summary>
        /// Scope name
        /// </summary>
        public string Scope { get; private set; }

        /// <summary>
        /// FQN used by the compiler
        /// </summary>
        public string MangledName => Scope != null ? $"{this.Scope}__{this.Name}" : this.Name;


        public Symbol(string name, Type type, string scope = null)
        {
            this.Name = name;
            this.Type = type;
            this.Scope = scope;
        }
    }
}
