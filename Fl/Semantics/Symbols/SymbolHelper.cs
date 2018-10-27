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

            return System.Enum.Parse<Access>(token.Value, true);
        }

        internal static Storage GetStorage(Token token, Storage def = Storage.Immutable)
        {
            if (token == null)
                return def;

            var str = token.Value;

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
                if (token.Value == Bool.Instance.ToString())
                    return Bool.Instance;

                if (token.Value == Char.Instance.ToString())
                    return Char.Instance;

                if (token.Value == Int.Instance.ToString())
                    return Int.Instance;

                if (token.Value == Float.Instance.ToString())
                    return Float.Instance;

                if (token.Value == Double.Instance.ToString())
                    return Double.Instance;

                if (token.Value == Decimal.Instance.ToString())
                    return Decimal.Instance;

                if (token.Value == String.Instance.ToString())
                    return String.Instance;

                if (token.Value == Null.Instance.ToString())
                    return Null.Instance;

                // Support complex types:
                if (token.Value == "func" || token.Value == "tuple")
                    return inferrer?.NewAnonymousType();

                if (symtable.HasSymbol(token.Value))
                {
                    var stype = symtable.GetSymbol(token.Value).Type;
                    if (stype is Class ctype)
                        return new ClassInstance(ctype);
                    return stype;
                }

                var type = new ClassInstance(new Class(token.Value));
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
