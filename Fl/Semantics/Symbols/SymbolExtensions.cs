using Fl.Semantics.Exceptions;
using Fl.Semantics.Symbols.Types;
using Fl.Semantics.Symbols.Types.Specials;
using Fl.Semantics.Symbols.Values;

namespace Fl.Semantics.Symbols
{
    public static class SymbolExtensions
    {
        /// <summary>
        /// Returns true if the symbol can be used in an assignment expression
        /// </summary>
        /// <param name="self">Symbol</param>
        /// <returns></returns>
        public static bool IsAssignable(this ISymbol self)
        {
            if (self is IBoundSymbol)
                return true;

            var selfType = self as ITypeSymbol;

            if (selfType == null)
                return false;

            return !selfType.IsOfType<VoidSymbol>() && !selfType.IsOfType<NoneSymbol>();
        }

        /// <summary>
        /// Checks if the symbol is of the specified type
        /// </summary>
        /// <typeparam name="T">Type to check</typeparam>
        /// <param name="self">Target symbol</param>
        /// <returns></returns>
        public static bool IsOfType<T>(this ISymbol self) where T : ITypeSymbol
        {
            return self is T;
        }

        /// <summary>
        /// Returns the type information of an assignable symbol
        /// </summary>
        /// <param name="self">Symbol to retrieve the type information</param>
        /// <returns></returns>
        public static ITypeSymbol GetTypeSymbol(this ISymbol self)
        {
            if (!self.IsAssignable())
                throw new SymbolException($"'{self.Name}' cannot be used as a value");

            if (self is IBoundSymbol bs)
                return bs.TypeSymbol;

            return self as ITypeSymbol;
        }
    }
}
