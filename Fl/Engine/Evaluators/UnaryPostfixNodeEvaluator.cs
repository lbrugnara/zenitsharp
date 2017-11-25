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
    class UnaryPostfixNodeEvaluator : OperandUnaryNode, INodeEvaluator<AstEvaluator, AstUnaryPostfixNode, FlObject>
    {
        public FlObject Evaluate(AstEvaluator evaluator, AstUnaryPostfixNode unary)
        {
            FlObject symbolValue = unary.Left.Exec(evaluator);

            Symbol symbol = GetSymbol(evaluator, unary);
            if (symbol == null || symbol.SymbolType != SymbolType.Variable)
                throw new AstWalkerException($"The operand of an increment/decrement operator must be a variable");

            switch (unary.Operator.Type)
            {
                case TokenType.Increment:
                    return symbolValue.PostIncrement();
                case TokenType.Decrement:
                    return symbolValue.PostDecrement();
            }
            throw new AstWalkerException("Unknown error");
        }
    }
}
