using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Objects;
using Fl.Parser;
using System;
using System.Linq;

namespace Fl.Engine.Symbols.Types
{
    public class OperandType
    {
        private string TypeName { get; }

        public OperandType(string typename)
        {
            this.TypeName = typename;
        }

        internal static OperandType Null => new OperandType("null");

        internal static OperandType Auto => new OperandType("var");

        public override bool Equals(object obj)
        {
            return this.TypeName == (obj as OperandType)?.TypeName;
        }

        public static bool operator ==(OperandType type1, OperandType type2)
        {
            if (object.ReferenceEquals(type1, null))
                return object.ReferenceEquals(type2, null);

            return type1.Equals(type2);
        }

        public static bool operator !=(OperandType type1, OperandType type2)
        {
            return !(type1 == type2);
        }

        public override string ToString()
        {
            return this.TypeName;
        }

        internal static OperandType FromToken(Token token)
        {
            if (token.Type == TokenType.Identifier)
                return new OperandType(token.Value.ToString());

            switch (token.Type)
            {
                case TokenType.Boolean:
                    return new OperandType("bool");

                case TokenType.Char:
                    return new OperandType("char");

                case TokenType.Integer:
                    return new OperandType("int");

                case TokenType.Float:
                    return new OperandType("float");

                case TokenType.Double:
                    return new OperandType("double");

                case TokenType.Decimal:
                    return new OperandType("decimal");

                case TokenType.String:
                    return new OperandType("string");

                case TokenType.Variable:
                    return new OperandType("var");

                case TokenType.Null:
                    return new OperandType("null");
            }
            throw new SymbolException($"Unrecognized literal {token.Type}");
        }
    }
}
