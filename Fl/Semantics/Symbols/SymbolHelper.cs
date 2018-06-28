// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Types;
using Fl.Syntax;
using Fl.Semantics.Exceptions;
using System;
using Fl.Semantics.Symbols;
using Fl.Semantics.Inferrers;

namespace Fl.Semantics.Types
{
    public class SymbolHelper
    {
        internal static Access GetAccess(Token token)
        {
            if (token == null)
                return Access.Public;

            return System.Enum.Parse<Access>(token.Value.ToString(), true);
        }

        internal static Storage GetStorage(Token token, Storage def = Storage.Immutable)
        {
            if (token == null)
                return def;

            var str = token.Value.ToString();

            switch (str)
            {
                case "mut":
                    return Storage.Mutable;

                case "const":
                    return Storage.Constant;
            }

            throw new Exception($"Unhandled storage type {str}");
        }

        internal static Type GetType(SymbolTable symtable, Token token)
        {
            return GetType(symtable, null, token);
        }

        internal static Type GetType(SymbolTable symtable, TypeInferrer inferrer, Token token)
        {
            if (token.Type == TokenType.Identifier)
            {
                string val = token.Value.ToString();

                if (val == Bool.Instance.ToString())
                    return Bool.Instance;

                if (val == Char.Instance.ToString())
                    return Char.Instance;

                if (val == Int.Instance.ToString())
                    return Int.Instance;

                if (val == Float.Instance.ToString())
                    return Float.Instance;

                if (val == Double.Instance.ToString())
                    return Double.Instance;

                if (val == Decimal.Instance.ToString())
                    return Decimal.Instance;

                if (val == String.Instance.ToString())
                    return String.Instance;

                if (val == Null.Instance.ToString())
                    return Null.Instance;

                // Support complex types:
                if (val == "func" || val == "tuple")
                    return inferrer?.NewAnonymousType();

                if (symtable.HasSymbol(val))
                {
                    var stype = symtable.GetSymbol(val).Type;
                    if (stype is Class ctype)
                        return new ClassInstance(ctype);
                    return stype;
                }

                var type = new ClassInstance(new Class(val));
                symtable.AddUnresolvedType(type.Class.ClassName, token);
                return type;
            }

            switch (token.Type)
            {
                case TokenType.Boolean:
                    return Bool.Instance;

                case TokenType.Char:
                    return Char.Instance;

                case TokenType.Integer:
                    return Int.Instance;

                case TokenType.Float:
                    return Float.Instance;

                case TokenType.Double:
                    return Double.Instance;

                case TokenType.Decimal:
                    return Decimal.Instance;

                case TokenType.String:
                    return String.Instance;

                case TokenType.Null:
                    return Null.Instance;

                case TokenType.Variable:
                    return inferrer?.NewAnonymousType(); // Auto
            }

            throw new SymbolException($"Unrecognized literal {token.Type}");
        }
    }
}
