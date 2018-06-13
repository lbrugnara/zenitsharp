// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols.Types;
using Fl.Parser;
using Fl.Symbols.Exceptions;

namespace Fl.Symbols.Types
{
    public class TypeHelper
    {
        internal static Type FromToken(Token token)
        {
            if (token.Type == TokenType.Identifier)
            {
                string val = token.Value.ToString();

                if (val == Bool.Instance.ToString())
                    return Bool.Instance;

                else if (val == Char.Instance.ToString())
                    return Char.Instance;

                else if (val == Int.Instance.ToString())
                    return Int.Instance;

                else if (val == Float.Instance.ToString())
                    return Float.Instance;

                else if (val == Double.Instance.ToString())
                    return Double.Instance;

                else if (val == Decimal.Instance.ToString())
                    return Decimal.Instance;

                else if (val == String.Instance.ToString())
                    return String.Instance;

                else if (val == Null.Instance.ToString())
                    return Null.Instance;

                // TODO: Fix this once custom types are implemented
                throw new SymbolException($"Unrecognized identifier {token.Type}");
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
                    return null; // Auto
            }

            throw new SymbolException($"Unrecognized literal {token.Type}");
        }
    }
}
