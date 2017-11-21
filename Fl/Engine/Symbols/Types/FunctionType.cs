// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

namespace Fl.Engine.Symbols.Types
{
    public class FunctionType : ObjectType
    {
        private static FunctionType _Instance;

        private FunctionType() { }

        public static FunctionType Value => _Instance != null ? _Instance : (_Instance = new FunctionType());

        public override string ToString()
        {
            return "function";
        }
    }
}
