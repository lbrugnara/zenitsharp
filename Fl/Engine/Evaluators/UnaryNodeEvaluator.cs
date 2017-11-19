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
                    if (result.Type == ObjectType.Boolean)
                        return new FlObject(ObjectType.Boolean, !result.AsBool);
                    throw new AstWalkerException($"Operator '!' cannot be applied to operand of type {result.Type}");
                case TokenType.Minus:
                    switch (result.Type)
                    {
                        case ObjectType.Integer:
                            return new FlObject(ObjectType.Integer, -1 * result.AsInt);
                        case ObjectType.Double:
                            return new FlObject(ObjectType.Double, -1.0 * result.AsDouble);
                        case ObjectType.Decimal:
                            return new FlObject(ObjectType.Decimal, -1.0M * result.AsDecimal);
                        default:
                            throw new AstWalkerException($"Operator '-' cannot be applied to operand of type {result.Type}");
                    }
            }
            return result;
        }
    }
}
