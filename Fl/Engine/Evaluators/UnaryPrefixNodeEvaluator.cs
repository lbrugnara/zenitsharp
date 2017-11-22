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
    class UnaryPrefixNodeEvaluator : OperandUnaryNode, INodeEvaluator<AstEvaluator, AstUnaryPrefixNode, FlObject>
    {
        public FlObject Evaluate(AstEvaluator evaluator, AstUnaryPrefixNode unary)
        {
            FlObject symbolValue = unary.Left.Exec(evaluator);

            Symbol symbol = GetSymbol(evaluator, unary);
            if (symbol == null || symbol.Storage != StorageType.Variable)
                throw new AstWalkerException($"The operand of an increment/decrement operator must be a variable");

            FlOperand symboloperand = new FlOperand(symbolValue);
            switch (unary.Operator.Type)
            {
                case TokenType.Increment:
                    return symboloperand.PreIncrement();
                case TokenType.Decrement:
                    return symboloperand.PreDecrement();
            }
            throw new AstWalkerException("Unknown error");
        }
    }
}
