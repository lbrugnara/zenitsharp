// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Lang.Types;
using Fl.Parser;
using Fl.Symbols;

namespace Fl.Symbols
{
    public class TypeHelper
    {
        internal static Type FromToken(Token token)
        {
            if (token.Type == TokenType.Identifier)
                return new Type(token.Value.ToString());

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
                    return null; // Auto
            }

            throw new SymbolException($"Unrecognized literal {token.Type}");
        }
    }
}
