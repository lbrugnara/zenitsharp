// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Linq;

namespace Fl.Engine.Symbols
{
    public static class ObjecTypeExtensions
    {
        private static readonly ObjectType[] Numeric = new ObjectType[] { ObjectType.Integer, ObjectType.Double, ObjectType.Decimal };

        public static bool IsNumeric(this ObjectType type)
        {
            return Numeric.Contains(type);
        }
    }
}
