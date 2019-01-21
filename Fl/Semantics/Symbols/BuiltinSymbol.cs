namespace Fl.Semantics.Symbols
{
    public enum BuiltinSymbol
    {
        This,
        Return
    }

    public static class BuiltinSymbolExtensions
    {
        public static string GetName(this BuiltinSymbol self)
        {
            switch (self)
            {
                case BuiltinSymbol.This:
                    return "@this";

                case BuiltinSymbol.Return:
                    return "@ret";
            }

            return null;
        }
    }
}
