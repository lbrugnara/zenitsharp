// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Types;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Evaluators
{
    class UnaryNodeEvaluator : INodeEvaluator<AstEvaluator, AstUnaryNode, FlObject>
    {
        public FlObject Evaluate(AstEvaluator evaluator, AstUnaryNode unary)
        {
            FlObject result = unary.Left.Exec(evaluator);
            // If unary token is null, we just need the "Left" expression (primary)
            if (unary.Operator == null)
                return result;

            // Make sure types are correct for Negation and Negative (Minus)
            switch (unary.Operator.Type)
            {
                case TokenType.Not:
                    if (result.ObjectType != BoolType.Value)
                        throw new AstWalkerException($"Operator '!' cannot be applied to operand of type {result.ObjectType}");
                    result = new FlBoolean(!(result as FlBoolean).Value);
                    break;
                case TokenType.Minus:
                    FlOperand operand = new FlOperand(result);
                    result = operand.Negative();
                    break;
            }
            return result;
        }
    }
}
