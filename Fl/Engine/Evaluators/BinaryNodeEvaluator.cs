// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fl.Engine.Evaluators
{
    class BinaryNodeEvaluator : INodeEvaluator<AstEvaluator, AstBinaryNode, FlObject>
    {
        public FlObject Evaluate(AstEvaluator evaluator, AstBinaryNode binary)
        {
            Token token = binary.Operator;
            switch (token.Type)
            {
                case TokenType.Or:
                case TokenType.And:
                    return ExecLogicalOperator(evaluator, binary);
                case TokenType.GreatThan:
                case TokenType.GreatThanEqual:
                case TokenType.LessThan:
                case TokenType.LessThanEqual:
                    return ExecComparisonOperator(evaluator, binary);
                case TokenType.Equal:
                case TokenType.NotEqual:
                    return ExecEqualityOperator(evaluator, binary);
                case TokenType.Addition:
                case TokenType.Minus:
                    return ExecAdditionOperator(evaluator, binary);
                case TokenType.Multiplication:
                case TokenType.Division:
                    return ExecMultOperator(evaluator, binary);
            }
            throw new AstWalkerException($"Unhandled operator {token.Type}");
        }

        private FlObject ExecMultOperator(AstEvaluator evaluator,  AstBinaryNode binary)
        {
            FlObject leftres = binary.Left.Exec(evaluator);
            FlObject rightres = binary.Right.Exec(evaluator);

            if (!(leftres.ObjectType is NumericType) || !(rightres.ObjectType is NumericType))
            {
                throw new AstWalkerException($"Operator '{binary.Operator.Type}' cannot be applied to operands of type '{leftres.ObjectType}' and '{rightres.ObjectType}'");
            }

            switch (binary.Operator.Type)
            {
                case TokenType.Multiplication:
                    return leftres.Multiply(rightres);
                case TokenType.Division:
                    return leftres.Divide(rightres);
            }
            throw new AstWalkerException($"Unhandled operator {binary.Operator.Type}");
        }

        private FlObject ExecAdditionOperator(AstEvaluator evaluator,  AstBinaryNode binary)
        {
            FlObject leftres = binary.Left.Exec(evaluator);
            FlObject rightres = binary.Right.Exec(evaluator);

            if (!(leftres.ObjectType is NumericType) || !(rightres.ObjectType is NumericType))
            {
                if (!(leftres.ObjectType is StringType) && !(rightres.ObjectType is StringType))
                    throw new AstWalkerException($"Operator '{binary.Operator.Type}' cannot be applied to operands of type '{leftres.ObjectType}' and '{rightres.ObjectType}'");
            }

            switch (binary.Operator.Type)
            {
                case TokenType.Addition:
                    return leftres.Add(rightres);
                case TokenType.Minus:
                    return leftres.Subtract(rightres);
            }
            throw new AstWalkerException($"Unhandled operator {binary.Operator.Type}");
        }

        private FlObject ExecEqualityOperator(AstEvaluator evaluator,  AstBinaryNode binary)
        {
            FlObject leftres = binary.Left.Exec(evaluator);
            FlObject rightres = binary.Right.Exec(evaluator);

            if (leftres.ObjectType == NullType.Value && rightres.ObjectType == NullType.Value)
                return new FlBool(true);

            FlBool equals = leftres.Equals(rightres);

            switch (binary.Operator.Type)
            {
                case TokenType.Equal:
                    return equals;
                case TokenType.NotEqual:
                    equals.Value = !equals.Value;
                    return equals;
            }
            throw new AstWalkerException($"Unhandled operator {binary.Operator.Type}");
        }

        private FlObject ExecComparisonOperator(AstEvaluator evaluator,  AstBinaryNode binary)
        {
            FlObject leftres = binary.Left.Exec(evaluator);
            FlObject rightres = binary.Right.Exec(evaluator);

            switch (binary.Operator.Type)
            {
                case TokenType.GreatThan:
                    return leftres.GreatherThan(rightres);
                case TokenType.GreatThanEqual:
                    return leftres.GreatherThanEquals(rightres);
                case TokenType.LessThan:
                    return leftres.LesserThan(rightres);
                case TokenType.LessThanEqual:
                    return leftres.LesserThanEquals(rightres);
            }
            throw new AstWalkerException($"Operator '{binary.Operator.Type}' cannot be applied to operands of type '{leftres.ObjectType}' and '{rightres.ObjectType}'");
        }

        private FlObject ExecLogicalOperator(AstEvaluator evaluator,  AstBinaryNode binary)
        {
            FlObject leftres = binary.Left.Exec(evaluator);
            FlObject rightres = binary.Right.Exec(evaluator);

            bool l = leftres.ObjectType != BoolType.Value ? leftres.RawValue != null : (leftres as FlBool).Value;
            bool r = rightres.ObjectType != BoolType.Value ? rightres.RawValue != null : (rightres as FlBool).Value;

            switch (binary.Operator.Type)
            {
                case TokenType.And:
                    return new FlBool(l && r);
                case TokenType.Or:
                    return new FlBool(l || r);
            }
            throw new AstWalkerException($"Unhandled operator {binary.Operator.Type}");
        }
    }
}
