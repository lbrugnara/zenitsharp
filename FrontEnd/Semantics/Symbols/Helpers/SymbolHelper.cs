// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Syntax;
using Zenit.Semantics.Exceptions;
using System;
using Zenit.Semantics.Symbols;
using Zenit.Semantics.Inferrers;
using Zenit.Semantics.Symbols.Types;
using Zenit.Semantics.Symbols.Types.Specials;
using Zenit.Semantics.Symbols.Types.Primitives;

namespace Zenit.Semantics.Types
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

        internal static BuiltinType GetType(SymbolTable symtable, Token token)
        {
            return GetType(symtable, null, token);
        }

        internal static BuiltinType GetType(SymbolTable symtable, TypeInferrer inferrer, Token token)
        {
            return GetTypeSymbol(symtable, inferrer, token).BuiltinType;
        }

        internal static IType GetTypeSymbol(SymbolTable symtable, TypeInferrer inferrer, Token token)
        {
            // If the token has a reference to an identifier, the types come from the explicit annotation
            if (token.Type == TokenType.Identifier)
            {
                if (token.Value == BuiltinType.Bool.GetName())
                    return new Primitive(BuiltinType.Bool, symtable.CurrentScope);

                if (token.Value == BuiltinType.Char.GetName())
                    return new Primitive(BuiltinType.Char, symtable.CurrentScope);

                if (token.Value == BuiltinType.Int.GetName())
                    return new Primitive(BuiltinType.Int, symtable.CurrentScope);

                if (token.Value == BuiltinType.Float.GetName())
                    return new Primitive(BuiltinType.Float, symtable.CurrentScope);

                if (token.Value == BuiltinType.Double.GetName())
                    return new Primitive(BuiltinType.Double, symtable.CurrentScope);

                if (token.Value == BuiltinType.Decimal.GetName())
                    return new Primitive(BuiltinType.Decimal, symtable.CurrentScope);

                if (token.Value == BuiltinType.String.GetName())
                    return new Primitive(BuiltinType.String, symtable.CurrentScope);

                // Support complex types:
                if (token.Value == "func" || token.Value == "tuple")
                    return inferrer?.NewAnonymousType();

                if (symtable.HasVariableSymbol(token.Value))
                {
                    /*var typeInfo = symtable.Get(token.Value).ITypeSymbol;
                    if (typeInfo.Type is Class ctype)
                        return new ITypeSymbol(new ClassInstance(ctype));
                    return typeInfo;*/
                }

                /*var type = new ClassInstance(new Class(token.Value));
                symtable.AddUnresolvedType(type.Class.ClassName, token);
                return new ITypeSymbol(type);*/
                throw new Exception($"Unknown type '{token.Value}'");
            }


            // Getting the type from a literal value or an 'auto' property
            switch (token.Type)
            {
                case TokenType.Boolean:
                    return new Primitive(BuiltinType.Bool, symtable.CurrentScope);

                case TokenType.Char:
                    return new Primitive(BuiltinType.Char, symtable.CurrentScope);

                case TokenType.Integer:
                    return new Primitive(BuiltinType.Int, symtable.CurrentScope);

                case TokenType.Float:
                    return new Primitive(BuiltinType.Float, symtable.CurrentScope);

                case TokenType.Double:
                    return new Primitive(BuiltinType.Double, symtable.CurrentScope);

                case TokenType.Decimal:
                    return new Primitive(BuiltinType.Decimal, symtable.CurrentScope);

                case TokenType.String:
                    return new Primitive(BuiltinType.String, symtable.CurrentScope);

                case TokenType.Variable:
                    return new None();
                    //return inferrer?.NewAnonymousType(); // Auto
            }

            throw new SymbolException($"Unrecognized literal {token.Type}");
        }
    }
}
