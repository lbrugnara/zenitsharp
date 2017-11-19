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
    class UnaryPostfixNodeEvaluator : OperandUnaryNode, INodeEvaluator<AstEvaluator, AstUnaryPostfixNode, FlObject>
    {
        public FlObject Evaluate(AstEvaluator evaluator, AstUnaryPostfixNode unary)
        {
            FlObject result = unary.Left.Exec(evaluator);

            switch (unary.Operator.Type)
            {
                case TokenType.Increment:
                {
                    Symbol symbol = GetSymbol(evaluator, unary);
                    if (symbol == null || symbol.Storage != StorageType.Variable)
                        throw new AstWalkerException($"The operand of an increment operator must be a variable");

                    object objval = null;
                    switch (result.Type)
                    {
                        case ObjectType.Integer:
                            symbol.Binding.Value = (int)(objval = result.AsInt) + 1;
                            return new FlObject(result.Type, objval);
                        case ObjectType.Double:
                            symbol.Binding.Value = (double)(objval = result.AsDouble) + 1.0;
                            return new FlObject(result.Type, objval);
                        case ObjectType.Decimal:
                            symbol.Binding.Value = (decimal)(objval = result.AsDecimal) + 1.0M;
                            return new FlObject(result.Type, objval);
                        default:
                            throw new AstWalkerException($"Operator ++ cannot be applied to operand of type {result.Type}");
                    }
                }
                case TokenType.Decrement:
                {
                    Symbol symbol = GetSymbol(evaluator, unary);
                    if (symbol == null || symbol.Storage != StorageType.Variable)
                        throw new AstWalkerException($"The operand of an increment operator must be a variable");

                    object objval = null;
                    switch (result.Type)
                    {
                        case ObjectType.Integer:
                            symbol.Binding.Value = (int)(objval = result.AsInt) - 1;
                            return new FlObject(result.Type, objval);
                        case ObjectType.Double:
                            result.Value = (double)(objval = result.AsDouble) - 1.0;
                            return new FlObject(result.Type, objval);
                        case ObjectType.Decimal:
                            result.Value = (decimal)(objval = result.AsDecimal) - 1.0M;
                            return new FlObject(result.Type, objval);
                        default:
                            throw new AstWalkerException($"Operator -- cannot be applied to operand of type {result.Type}");
                    }
                }
            }
            return result;
        }
    }
}
