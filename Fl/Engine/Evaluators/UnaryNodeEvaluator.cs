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
    class UnaryNodeEvaluator : INodeEvaluator<AstEvaluator, AstUnaryNode, Symbol>
    {
        public Symbol Evaluate(AstEvaluator evaluator, AstUnaryNode unary)
        {
            Symbol result = unary.Left.Exec(evaluator);
            // If unary token is null, we just need the "Left" expression (primary)
            if (unary.Operator == null)
                return result;

            // Make sure types are correct for Negation and Negative (Minus)
            switch (unary.Operator.Type)
            {
                case TokenType.Not:
                    if (result.DataType == SymbolType.Boolean)
                        return new Symbol(SymbolType.Boolean, !result.AsBool);
                    throw new AstWalkerException($"Operator '!' cannot be applied to operand of type {result.DataType}");
                case TokenType.Minus:
                    switch (result.DataType)
                    {
                        case SymbolType.Integer:
                            return new Symbol(SymbolType.Integer, -1 * result.AsInt);
                        case SymbolType.Double:
                            return new Symbol(SymbolType.Double, -1.0 * result.AsDouble);
                        case SymbolType.Decimal:
                            return new Symbol(SymbolType.Decimal, -1.0M * result.AsDecimal);
                        default:
                            throw new AstWalkerException($"Operator '-' cannot be applied to operand of type {result.DataType}");
                    }
            }
            return result;
        }
    }
}
