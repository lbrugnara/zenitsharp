using Fl.Symbols;
using Fl.Lang.Types;
using System.Collections.Generic;
using System.Text;

namespace Fl.TypeChecking.Inferrers
{
    public class InferredType
    {
        public Type Type { get; set; }
        public Symbol Symbol { get; set; }
    }
}
