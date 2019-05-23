// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Semantics.Symbols.Types;

namespace Zenit.Semantics.Symbols.Variables
{
    /// <summary>
    /// Represents a value symbol that is bound to a variable
    /// </summary>
    public interface IVariable : ISymbol
    {
        /// <summary>
        /// Bound symbol's type
        /// </summary>
        IType TypeSymbol { get; }

        /// <summary>
        /// Access level to the bound variable
        /// </summary>
        Access Access { get; }

        /// <summary>
        /// Storage type of the bound variable
        /// </summary>
        Storage Storage { get; }

        /// <summary>
        /// Change the symbol's underlying type
        /// </summary>
        /// <param name="type"></param>
        void ChangeType(IType type);
    }
}
