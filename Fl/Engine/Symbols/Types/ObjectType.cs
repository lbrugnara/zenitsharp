// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Objects;
using Fl.Parser;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Symbols.Types
{
    public abstract class ObjectType
    {
        public abstract string Name { get; }

        public abstract string ClassName { get; }

        public override string ToString()
        {
            return Name;
        }

        public abstract object RawDefaultValue();

        public abstract FlObject DefaultValue();

        public abstract FlObject NewValue(object o);

        public static ObjectType GetFromTokenType(TokenType ttype)
        {
            ObjectType dtype = null;
            switch (ttype)
            {
                case TokenType.Boolean:
                    dtype = BoolType.Value;
                    break;
                case TokenType.Char:
                    dtype = CharType.Value;
                    break;
                case TokenType.Integer:
                    dtype = IntegerType.Value;
                    break;
                case TokenType.Float:
                    dtype = FloatType.Value;
                    break;
                case TokenType.Double:
                    dtype = DoubleType.Value;
                    break;
                case TokenType.Decimal:
                    dtype = DecimalType.Value;
                    break;
                case TokenType.String:
                    dtype = StringType.Value;
                    break;
                case TokenType.Null:
                    dtype = NullType.Value;
                    break;
            }
            return dtype;
        }

        public static ObjectType GetFromTypeName(string typename)
        {
            if (typename == BoolType.Value.ClassName)
                return BoolType.Value;

            if (typename == CharType.Value.ClassName)
                return CharType.Value;

            if (typename == IntegerType.Value.ClassName)
                return IntegerType.Value;

            if (typename == FloatType.Value.ClassName)
                return FloatType.Value;

            if (typename == DoubleType.Value.ClassName)
                return DoubleType.Value;

            if (typename == DecimalType.Value.ClassName)
                return DecimalType.Value;

            if (typename == StringType.Value.ClassName)
                return StringType.Value;

            if (typename == NullType.Value.ClassName)
                return NullType.Value;

            return null;
        }
    }
}
