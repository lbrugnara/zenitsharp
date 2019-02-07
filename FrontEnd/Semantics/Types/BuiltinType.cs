// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using System;

namespace Zenit.Semantics.Types
{
    public enum BuiltinType
    {
        // Special types
        None,
        Void,
        Anonymous,

        // Primitive types (Number types and String)
        Char,
        Bool,
        Int,
        Float,
        Double,
        Decimal,
        Number,

        String,

        // Complex types
        Function,
        Tuple,
        Class,
        
        Object
    };

    public static class BuiltinTypeExtensions
    {
        public static string GetName(this BuiltinType type)
        {
            switch (type)
            {
                case BuiltinType.None:
                    return "none";
                case BuiltinType.Void:
                    return "void";
                case BuiltinType.Char:
                    return "char";
                case BuiltinType.Bool:
                    return "bool";
                case BuiltinType.Int:
                    return "int";
                case BuiltinType.Float:
                    return "float";
                case BuiltinType.Double:
                    return "double";
                case BuiltinType.Decimal:
                    return "decimal";
                case BuiltinType.Number:
                    return "number";
                case BuiltinType.String:
                    return "string";
                case BuiltinType.Function:
                    return "func";
                case BuiltinType.Tuple:
                    return "tuple";
                case BuiltinType.Class:
                    return "class";
                case BuiltinType.Object:
                case BuiltinType.Anonymous:
                    return "object";
            }

            throw new Exception($"Unknown built-in type '{type}'");
        }

        public static bool IsPrimitive(this BuiltinType type)
        {
            switch (type)
            {
                case BuiltinType.Char:
                case BuiltinType.Bool:
                case BuiltinType.Int:
                case BuiltinType.Float:
                case BuiltinType.Double:
                case BuiltinType.Decimal:
                case BuiltinType.Number:
                case BuiltinType.String:
                    return true;
            }
            return false;
        }

        public static bool IsNamedType(this BuiltinType type)
        {
            switch (type)
            {
                case BuiltinType.Char:
                case BuiltinType.Bool:
                case BuiltinType.Int:
                case BuiltinType.Float:
                case BuiltinType.Double:
                case BuiltinType.Decimal:
                case BuiltinType.Number:
                case BuiltinType.String:
                case BuiltinType.Class:
                    return true;
            }
            return false;
        }

        public static bool IsStructuralType(this BuiltinType type)
        {
            switch (type)
            {
                case BuiltinType.Function:
                case BuiltinType.Tuple:
                case BuiltinType.Object:
                    return true;
            }
            return false;
        }
    }
}
