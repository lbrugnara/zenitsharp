// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Evaluators
{
    class UnaryNodeEvaluator : INodeEvaluator<AstEvaluator, AstUnaryNode, ScopeEntry>
    {
        public ScopeEntry Evaluate(AstEvaluator evaluator, AstUnaryNode unary)
        {
            ScopeEntry result = unary.Left.Exec(evaluator);
            // If unary token is null, we just need the "Left" expression (primary)
            if (unary.Operator == null)
                return result;

            // Make sure types are correct for Negation and Negative (Minus)
            switch (unary.Operator.Type)
            {
                case TokenType.Not:
                    if (result.DataType == ScopeEntryType.Boolean)
                        return new ScopeEntry(ScopeEntryType.Boolean, !result.BoolValue);
                    throw new AstWalkerException($"Operator '!' cannot be applied to operand of type {result.DataType}");
                case TokenType.Minus:
                    switch (result.DataType)
                    {
                        case ScopeEntryType.Integer:
                            return new ScopeEntry(ScopeEntryType.Integer, -1 * result.IntValue);
                        case ScopeEntryType.Double:
                            return new ScopeEntry(ScopeEntryType.Double, -1.0 * result.DoubleValue);
                        case ScopeEntryType.Decimal:
                            return new ScopeEntry(ScopeEntryType.Decimal, -1.0M * result.DecimalValue);
                        default:
                            throw new AstWalkerException($"Operator '-' cannot be applied to operand of type {result.DataType}");
                    }
            }
            return result;
        }
    }
}
