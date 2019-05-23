using Zenit.Semantics.Exceptions;
using Zenit.Semantics.Symbols.Types;
using Zenit.Semantics.Symbols.Types.Specials;
using Zenit.Semantics.Symbols.Variables;

namespace Zenit.Semantics.Symbols
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
            if (self is IVariable)
                return true;

            var selfType = self as IType;

            if (selfType == null)
                return false;

            return !selfType.IsOfType<Void>() && !selfType.IsOfType<None>();
        }

        /// <summary>
        /// Checks if the symbol is of the specified type
        /// </summary>
        /// <typeparam name="T">Type to check</typeparam>
        /// <param name="self">Target symbol</param>
        /// <returns></returns>
        public static bool IsOfType<T>(this ISymbol self) where T : IType
        {
            return self is T;
        }

        /// <summary>
        /// Returns the type information of an assignable symbol
        /// </summary>
        /// <param name="self">Symbol to retrieve the type information</param>
        /// <returns></returns>
        public static IType GetTypeSymbol(this ISymbol self)
        {
            if (!self.IsAssignable())
                throw new SymbolException($"'{self.Name}' cannot be used as a value");

            if (self is IVariable bs)
                return bs.TypeSymbol;

            return self as IType;
        }
    }
}
