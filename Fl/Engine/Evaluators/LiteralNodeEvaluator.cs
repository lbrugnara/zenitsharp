// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Engine.Symbols;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Evaluators
{
    class LiteralNodeEvaluator : INodeEvaluator<AstEvaluator, AstLiteralNode, Symbol>
    {
        public Symbol Evaluate(AstEvaluator evaluator, AstLiteralNode literal)
        {
            SymbolType? dataType = null;
            object val = literal.Primary.Value;
            switch (literal.Primary.Type)
            {
                case TokenType.Boolean:
                    val = bool.Parse(literal.Primary.Value.ToString());
                    dataType = SymbolType.Boolean;
                    break;
                case TokenType.Integer:
                    val = int.Parse(literal.Primary.Value.ToString());
                    dataType = SymbolType.Integer;
                    break;
                case TokenType.Double:
                    val = double.Parse(literal.Primary.Value.ToString());
                    dataType = SymbolType.Double;
                    break;
                case TokenType.Decimal:
                    val = decimal.Parse(literal.Primary.Value.ToString());
                    dataType = SymbolType.Decimal;
                    break;
                case TokenType.String:
                    val = literal.Primary.Value.ToString();
                    dataType = SymbolType.String;
                    break;
                case TokenType.Identifier:
                    return evaluator.CurrentScope[literal.Primary.Value];
                case TokenType.Null:
                    val = null;
                    dataType = SymbolType.Null;
                    break;
            }
            // TODO: Variables and Constants need to resolve the evtype too
            return new Symbol(dataType.Value, val);
        }
    }
}
