using Fl.Symbols;
using Fl.Lang.Types;
using System.Collections.Generic;
using System.Text;

namespace Fl.TypeChecking.Inferrers
{
    public class InferredType
    {
        public Type Type { get; private set; }
        public string Symbol { get; private set; }

        public InferredType(Type type, string symbol = null)
        {
            this.Type = type ?? throw new System.ArgumentNullException(nameof(type), "Inferred Type cannot be null");
            this.Symbol = symbol;
        }
    }
}
