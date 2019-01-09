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
                if (token.Value == BuiltinType.Bool.GetName())
                    return new TypeInfo(new Object(BuiltinType.Bool));

                if (token.Value == BuiltinType.Char.GetName())
                    return new TypeInfo(new Object(BuiltinType.Char));

                if (token.Value == BuiltinType.Int.GetName())
                    return new TypeInfo(new Object(BuiltinType.Int));

                if (token.Value == BuiltinType.Float.GetName())
                    return new TypeInfo(new Object(BuiltinType.Float));

                if (token.Value == BuiltinType.Double.GetName())
                    return new TypeInfo(new Object(BuiltinType.Double));

                if (token.Value == BuiltinType.Decimal.GetName())
                    return new TypeInfo(new Object(BuiltinType.Decimal));

                if (token.Value == BuiltinType.String.GetName())
                    return new TypeInfo(new Object(BuiltinType.String));

                // Support complex types:
                if (token.Value == "func" || token.Value == "tuple")
                    return inferrer?.NewAnonymousType();

                if (symtable.Contains(token.Value))
                {
                    /*var typeInfo = symtable.Get(token.Value).TypeInfo;
                    if (typeInfo.Type is Class ctype)
                        return new TypeInfo(new ClassInstance(ctype));
                    return typeInfo;*/
                }

                /*var type = new ClassInstance(new Class(token.Value));
                symtable.AddUnresolvedType(type.Class.ClassName, token);
                return new TypeInfo(type);*/
                throw new Exception($"Unknown type '{token.Value}'");
            }


            // Getting the type from a literal value or an 'auto' property
            switch (token.Type)
            {
                case TokenType.Boolean:
                    return new TypeInfo(new Object(BuiltinType.Bool));

                case TokenType.Char:
                    return new TypeInfo(new Object(BuiltinType.Char));

                case TokenType.Integer:
                    return new TypeInfo(new Object(BuiltinType.Int));

                case TokenType.Float:
                    return new TypeInfo(new Object(BuiltinType.Float));

                case TokenType.Double:
                    return new TypeInfo(new Object(BuiltinType.Double));

                case TokenType.Decimal:
                    return new TypeInfo(new Object(BuiltinType.Decimal));

                case TokenType.String:
                    return new TypeInfo(new Object(BuiltinType.String));

                case TokenType.Variable:
                    return inferrer?.NewAnonymousType(); // Auto
            }

            throw new SymbolException($"Unrecognized literal {token.Type}");
        }
    }
}
