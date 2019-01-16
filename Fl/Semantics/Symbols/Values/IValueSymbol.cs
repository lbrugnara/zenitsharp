namespace Fl.Semantics.Symbols.Values
{
    /// <summary>
    /// Represents a symbol that can live bound or unbound to a variable name
    /// </summary>
    public interface IValueSymbol : ISymbol
    {
        /// <summary>
        /// Returns a representation of the value symbol
        /// </summary>
        /// <returns></returns>
        string ToValueString();
    }
}
