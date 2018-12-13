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

        internal static Object GetType(SymbolTable symtable, Token token)
        {
            return GetType(symtable, null, token);
        }

        internal static Object GetType(SymbolTable symtable, TypeInferrer inferrer, Token token)
        {
            return GetTypeInfo(symtable, inferrer, token).Type;
        }

        internal static TypeInfo GetTypeInfo(SymbolTable symtable, TypeInferrer inferrer, Token token)
        {
            // If the token has a reference to an identifier, the types come from the explicit annotation
            if (token.Type == TokenType.Identifier)
            {
                if (token.Value == Bool.Instance.ToString())
                    return new TypeInfo(Bool.Instance);

                if (token.Value == Char.Instance.ToString())
                    return new TypeInfo(Char.Instance);

                if (token.Value == Int.Instance.ToString())
                    return new TypeInfo(Int.Instance);

                if (token.Value == Float.Instance.ToString())
                    return new TypeInfo(Float.Instance);

                if (token.Value == Double.Instance.ToString())
                    return new TypeInfo(Double.Instance);

                if (token.Value == Decimal.Instance.ToString())
                    return new TypeInfo(Decimal.Instance);

                if (token.Value == String.Instance.ToString())
                    return new TypeInfo(String.Instance);

                // Support complex types:
                if (token.Value == "func" || token.Value == "tuple")
                    return inferrer?.NewAnonymousType();

                if (symtable.HasSymbol(token.Value))
                {
                    var typeInfo = symtable.GetSymbol(token.Value).TypeInfo;
                    if (typeInfo.Type is Class ctype)
                        return new TypeInfo(new ClassInstance(ctype));
                    return typeInfo;
                }

                var type = new ClassInstance(new Class(token.Value));
                symtable.AddUnresolvedType(type.Class.ClassName, token);
                return new TypeInfo(type);
            }


            // Getting the type from a literal value or an 'auto' property
            switch (token.Type)
            {
                case TokenType.Boolean:
                    return new TypeInfo(Bool.Instance);

                case TokenType.Char:
                    return new TypeInfo(Char.Instance);

                case TokenType.Integer:
                    return new TypeInfo(Int.Instance);

                case TokenType.Float:
                    return new TypeInfo(Float.Instance);

                case TokenType.Double:
                    return new TypeInfo(Double.Instance);

                case TokenType.Decimal:
                    return new TypeInfo(Decimal.Instance);

                case TokenType.String:
                    return new TypeInfo(String.Instance);

                case TokenType.Variable:
                    return inferrer?.NewAnonymousType(); // Auto
            }

            throw new SymbolException($"Unrecognized literal {token.Type}");
        }
    }
}
