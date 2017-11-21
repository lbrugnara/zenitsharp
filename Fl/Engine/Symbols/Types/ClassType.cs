// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

namespace Fl.Engine.Symbols.Types
{
    public class ClassType : ObjectType
    {
        private static ClassType _Instance;

        private ClassType() { }

        public static ClassType Value => _Instance != null ? _Instance : (_Instance = new ClassType());

        public override string ToString()
        {
            return "class";
        }
    }
}
