// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

namespace Fl.Semantics.Symbols
{
    public enum ScopeType
    {
        /// <summary>
        /// Represents the root block in the chain of blocks
        /// </summary>
        Global,

        /// <summary>
        /// Represents common blocks between { and }
        /// </summary>
        Common,

        /// <summary>
        /// Represents blocks used by loop instructions like while and for.
        /// </summary>
        Loop,

        /// <summary>
        /// Represents a function's body block
        /// </summary>
        Function,

        /// <summary>
        /// Represents a package block
        /// </summary>
        Package
    }
}
