// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;
using Fl.Parser;
using Fl.Parser.Ast;
using System;

namespace Fl.Engine.Evaluators
{
    class UnaryNodeEvaluator : INodeVisitor<AstEvaluator, AstUnaryNode, FlObject>
    {
        public FlObject Visit(AstEvaluator evaluator, AstUnaryNode unary)
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
                    result = result.Not();
                    break;
                case TokenType.Minus:                    
                    result = result.Negative();
                    break;
            }
            return result;
        }
    }
}
