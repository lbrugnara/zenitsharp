using Fl.Symbols;
using Fl.Lang.Types;

namespace Fl.TypeChecking.Inferrers
{
    public class InferredType
    {
        public Type Type { get; private set; }
        public Symbol Symbol { get; private set; }

        public InferredType(Type type, Symbol symbol = null)
        {
            this.Type = type ?? throw new System.ArgumentNullException(nameof(type), "Inferred Type cannot be null");
            this.Symbol = symbol;
        }
    }
}
