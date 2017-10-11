// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Evaluators
{
    class LiteralNodeEvaluator : INodeEvaluator<AstEvaluator, AstLiteralNode, ScopeEntry>
    {
        public ScopeEntry Evaluate(AstEvaluator evaluator, AstLiteralNode literal)
        {
            ScopeEntryType? dataType = null;
            object val = literal.Primary.Value;
            switch (literal.Primary.Type)
            {
                case TokenType.Boolean:
                    val = bool.Parse(literal.Primary.Value.ToString());
                    dataType = ScopeEntryType.Boolean;
                    break;
                case TokenType.Integer:
                    val = int.Parse(literal.Primary.Value.ToString());
                    dataType = ScopeEntryType.Integer;
                    break;
                case TokenType.Double:
                    val = double.Parse(literal.Primary.Value.ToString());
                    dataType = ScopeEntryType.Double;
                    break;
                case TokenType.Decimal:
                    val = decimal.Parse(literal.Primary.Value.ToString());
                    dataType = ScopeEntryType.Decimal;
                    break;
                case TokenType.String:
                    val = literal.Primary.Value.ToString();
                    dataType = ScopeEntryType.String;
                    break;
                case TokenType.Identifier:
                    return evaluator.CurrentScope[literal.Primary.Value];
                case TokenType.Null:
                    val = null;
                    dataType = ScopeEntryType.Null;
                    break;
            }
            // TODO: Variables and Constants need to resolve the evtype too
            return new ScopeEntry(dataType.Value, val);
        }
    }
}
