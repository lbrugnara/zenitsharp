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
    class LiteralNodeEvaluator : INodeEvaluator<AstEvaluator, AstLiteralNode, FlObject>
    {
        public FlObject Evaluate(AstEvaluator evaluator, AstLiteralNode literal)
        {
            ObjectType? dataType = null;
            object val = literal.Primary.Value;
            switch (literal.Primary.Type)
            {
                case TokenType.Boolean:
                    val = bool.Parse(literal.Primary.Value.ToString());
                    dataType = ObjectType.Boolean;
                    break;
                case TokenType.Integer:
                    val = int.Parse(literal.Primary.Value.ToString());
                    dataType = ObjectType.Integer;
                    break;
                case TokenType.Double:
                    val = double.Parse(literal.Primary.Value.ToString());
                    dataType = ObjectType.Double;
                    break;
                case TokenType.Decimal:
                    val = decimal.Parse(literal.Primary.Value.ToString());
                    dataType = ObjectType.Decimal;
                    break;
                case TokenType.String:
                    val = literal.Primary.Value.ToString();
                    dataType = ObjectType.String;
                    break;
                case TokenType.Identifier:
                    return evaluator.Symtable.GetSymbol(literal.Primary.Value).Binding;
                case TokenType.Null:
                    val = null;
                    dataType = ObjectType.Null;
                    break;
            }
            return new FlObject(dataType.Value, val);
        }
    }
}
