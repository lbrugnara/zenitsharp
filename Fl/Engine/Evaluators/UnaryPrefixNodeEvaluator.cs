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
    class UnaryPrefixNodeEvaluator : OperandUnaryNode, INodeEvaluator<AstEvaluator, AstUnaryPrefixNode, FlObject>
    {
        public FlObject Evaluate(AstEvaluator evaluator, AstUnaryPrefixNode unary)
        {
            FlObject result = unary.Left.Exec(evaluator);
            switch (unary.Operator.Type)
            {
                case TokenType.Increment:
                {
                    Symbol symbol = GetSymbol(evaluator, unary);
                    if (symbol == null || symbol.Storage != StorageType.Variable)
                        throw new AstWalkerException($"The operand of an increment operator must be a variable");

                    bool isPrefix = unary is AstUnaryPrefixNode;
                    object objval = null;
                    switch (result.Type)
                    {
                        case ObjectType.Integer:
                            symbol.Binding.Value = objval = result.AsInt + 1;
                            return new FlObject(result.Type, objval);
                        case ObjectType.Double:
                            symbol.Binding.Value = objval = result.AsDouble + 1.0;
                            return new FlObject(result.Type, objval);
                        case ObjectType.Decimal:
                            symbol.Binding.Value = objval = result.AsDecimal + 1.0M;
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

                    bool isPrefix = unary is AstUnaryPrefixNode;
                        object objval = null;
                    switch (result.Type)
                    {
                        case ObjectType.Integer:
                            symbol.Binding.Value = objval = result.AsInt - 1;
                            return new FlObject(result.Type, objval);
                        case ObjectType.Double:
                            symbol.Binding.Value = objval = result.AsDouble - 1.0;
                            return new FlObject(result.Type, objval);
                        case ObjectType.Decimal:
                            symbol.Binding.Value = objval = result.AsDecimal - 1.0M;
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
