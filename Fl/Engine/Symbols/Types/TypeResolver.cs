using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Objects;
using Fl.Parser;
using System.Linq;

namespace Fl.Engine.Symbols.Types
{/*
    public class TypeResolver
    {
        public string TypeName { get; }

        public TypeResolver(string typename)
        {
            this.TypeName = typename;
        }

        public FlType Resolve(SymbolTable st)
        {
            string[] names = this.TypeName.Split('.');
            
            // Get the root value
            FlObject value = st.GetSymbol(names[0]).Binding;

            if (names.Length > 1)
            {
                FlObject tmpval = value;
                foreach (var name in names.Skip(1))
                {
                    if (tmpval.Type == FlNamespaceType.Instance)
                    {
                        tmpval = (tmpval as FlNamespace)[name].Binding;
                    }
                }
                value = tmpval;
            }

            return value as FlType;
        }

        public static TypeResolver GetTypeResolverFromToken(Token token)
        {
            string typeName;
            if (token.Type == TokenType.Identifier)
            {
                typeName = token.Value.ToString();
            }
            else
            {
                switch (token.Type)
                {
                    case TokenType.Boolean:
                        typeName = FlBoolType.Instance.Name;
                        break;
                    case TokenType.Char:
                        typeName = FlCharType.Instance.Name;
                        break;
                    case TokenType.Integer:
                        typeName = FlIntType.Instance.Name;
                        break;
                    case TokenType.Float:
                        typeName = FlFloatType.Instance.Name;
                        break;
                    case TokenType.Double:
                        typeName = FlDoubleType.Instance.Name;
                        break;
                    case TokenType.Decimal:
                        typeName = FlDecimalType.Instance.Name;
                        break;
                    case TokenType.String:
                        typeName = FlStringType.Instance.Name;
                        break;
                    case TokenType.Variable:
                    case TokenType.Null:
                        typeName = FlNullType.Instance.Name;
                        break;
                    default:
                        throw new SymbolException($"Unrecognized literal {token.Type}");
                }
            }
            return new TypeResolver(typeName);
        }
    }*/
}
