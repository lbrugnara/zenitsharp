// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using Fl.Parser;
using Fl.Parser.Ast;
using System;

namespace Fl.Engine.Evaluators
{
    class UnaryPrefixNodeEvaluator : OperandUnaryNode, INodeEvaluator<AstEvaluator, AstUnaryPrefixNode, FlObject>
    {
        public FlObject Evaluate(AstEvaluator evaluator, AstUnaryPrefixNode unary)
        {
            FlObject symbolValue = unary.Left.Exec(evaluator);

            Symbol symbol = GetSymbol(evaluator, unary);
            if (symbol == null || symbol.SymbolType != SymbolType.Variable)
                throw new AstWalkerException($"The operand of an increment/decrement operator must be a variable");

            switch (unary.Operator.Type)
            {
                case TokenType.Increment:
                    return symbolValue.PreIncrement();
                case TokenType.Decrement:
                    return symbolValue.PreDecrement();
            }
            throw new AstWalkerException("Unknown error");
        }
    }
}
