// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Evaluators
{
    class LiteralNodeEvaluator : INodeEvaluator<AstEvaluator, AstLiteralNode, FlObject>
    {
        public FlObject Evaluate(AstEvaluator evaluator, AstLiteralNode literal)
        {
            switch (literal.Primary.Type)
            {
                case TokenType.Boolean:
                    return new FlBool(bool.Parse(literal.Primary.Value.ToString()));
                case TokenType.Integer:
                    return new FlInteger(int.Parse(literal.Primary.Value.ToString()));
                case TokenType.Double:
                    return new FlDouble(double.Parse(literal.Primary.Value.ToString()));
                case TokenType.Decimal:
                    return new FlDecimal(decimal.Parse(literal.Primary.Value.ToString()));
                case TokenType.String:
                    return new FlString(literal.Primary.Value.ToString());
                case TokenType.Identifier:
                    return evaluator.Symtable.GetSymbol(literal.Primary.Value).Binding;
            }
            return FlNull.Value;
        }
    }
}
