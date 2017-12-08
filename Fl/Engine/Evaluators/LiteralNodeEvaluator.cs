// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Objects;
using Fl.Parser;
using Fl.Parser.Ast;
using System;

namespace Fl.Engine.Evaluators
{
    class LiteralNodeEvaluator : INodeEvaluator<AstEvaluator, AstLiteralNode, FlObject>
    {
        public FlObject Evaluate(AstEvaluator evaluator, AstLiteralNode literal)
        {
            switch (literal.Literal.Type)
            {
                case TokenType.Boolean:
                    return new FlBool(bool.Parse(literal.Literal.Value.ToString()));
                case TokenType.Integer:
                    return new FlInteger(int.Parse(literal.Literal.Value.ToString()));
                case TokenType.Double:
                    return new FlDouble(double.Parse(literal.Literal.Value.ToString()));
                case TokenType.Decimal:
                    return new FlDecimal(decimal.Parse(literal.Literal.Value.ToString()));
                case TokenType.String:
                    return new FlString(literal.Literal.Value.ToString());
                case TokenType.Identifier:
                    return evaluator.Symtable.GetSymbol(literal.Literal.Value).Binding;
            }
            return FlNull.Value;
        }
    }
}
