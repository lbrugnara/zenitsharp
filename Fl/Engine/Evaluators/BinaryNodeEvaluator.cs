// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fl.Engine.Evaluators
{
    class BinaryNodeEvaluator : INodeEvaluator<AstEvaluator, AstBinaryNode, ScopeEntry>
    {
        private static readonly ScopeEntryType[] Numeric = new ScopeEntryType[] { ScopeEntryType.Integer, ScopeEntryType.Double, ScopeEntryType.Decimal };

        public ScopeEntry Evaluate(AstEvaluator evaluator, AstBinaryNode binary)
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

        private ScopeEntry ExecMultOperator(AstEvaluator evaluator,  AstBinaryNode binary)
        {
            ScopeEntry leftres = binary.Left.Exec(evaluator);
            ScopeEntry rightres = binary.Right.Exec(evaluator);

            if (!Numeric.Contains(leftres.DataType) || !Numeric.Contains(rightres.DataType))
            {
                throw new AstWalkerException($"Operator '{binary.Operator.Type}' cannot be applied to operands of type '{leftres.DataType}' and '{rightres.DataType}'");
            }

            switch (binary.Operator.Type)
            {
                case TokenType.Multiplication:
                    if (leftres.DataType == ScopeEntryType.Decimal || rightres.DataType == ScopeEntryType.Decimal)
                        return new ScopeEntry(ScopeEntryType.Decimal, leftres.DecimalValue * rightres.DecimalValue);

                    if (leftres.DataType == ScopeEntryType.Double || rightres.DataType == ScopeEntryType.Double)
                        return new ScopeEntry(ScopeEntryType.Double, leftres.DoubleValue * rightres.DoubleValue);

                    return new ScopeEntry(ScopeEntryType.Integer, leftres.IntValue * rightres.IntValue);

                case TokenType.Division:
                    if (leftres.DataType == ScopeEntryType.Decimal || rightres.DataType == ScopeEntryType.Decimal)
                        return new ScopeEntry(ScopeEntryType.Decimal, leftres.DecimalValue / rightres.DecimalValue);

                    // Integer will be promoted to double
                    return new ScopeEntry(ScopeEntryType.Double, leftres.DoubleValue / rightres.DoubleValue);
            }
            throw new AstWalkerException($"Unhandled operator {binary.Operator.Type}");
        }

        private ScopeEntry ExecAdditionOperator(AstEvaluator evaluator,  AstBinaryNode binary)
        {
            ScopeEntry leftres = binary.Left.Exec(evaluator);
            ScopeEntry rightres = binary.Right.Exec(evaluator);

            if (!Numeric.Contains(leftres.DataType) || !Numeric.Contains(rightres.DataType))
            {
                if (leftres.DataType != ScopeEntryType.String && rightres.DataType != ScopeEntryType.String)
                    throw new AstWalkerException($"Operator '{binary.Operator.Type}' cannot be applied to operands of type '{leftres.DataType}' and '{rightres.DataType}'");
            }

            switch (binary.Operator.Type)
            {
                case TokenType.Addition:
                    if (leftres.DataType == ScopeEntryType.Decimal || rightres.DataType == ScopeEntryType.Decimal)
                        return new ScopeEntry(ScopeEntryType.Decimal, leftres.DecimalValue + rightres.DecimalValue);

                    if (leftres.DataType == ScopeEntryType.Double || rightres.DataType == ScopeEntryType.Double)
                        return new ScopeEntry(ScopeEntryType.Double, leftres.DoubleValue + rightres.DoubleValue);

                    if (leftres.DataType == ScopeEntryType.String && rightres.DataType == ScopeEntryType.String)
                        return new ScopeEntry(ScopeEntryType.String, leftres.StrValue + rightres.StrValue);

                    return new ScopeEntry(ScopeEntryType.Integer, leftres.IntValue + rightres.IntValue);
                case TokenType.Minus:
                    if (leftres.DataType == ScopeEntryType.Decimal || rightres.DataType == ScopeEntryType.Decimal)
                        return new ScopeEntry(ScopeEntryType.Decimal, leftres.DecimalValue - rightres.DecimalValue);

                    if (leftres.DataType == ScopeEntryType.Double || rightres.DataType == ScopeEntryType.Double)
                        return new ScopeEntry(ScopeEntryType.Double, leftres.DoubleValue - rightres.DoubleValue);

                    return new ScopeEntry(ScopeEntryType.Integer, leftres.IntValue - rightres.IntValue);
            }
            throw new AstWalkerException($"Unhandled operator {binary.Operator.Type}");
        }

        private ScopeEntry ExecEqualityOperator(AstEvaluator evaluator,  AstBinaryNode binary)
        {
            ScopeEntry leftres = binary.Left.Exec(evaluator);
            ScopeEntry rightres = binary.Right.Exec(evaluator);

            if (leftres.IsNull && rightres.IsNull)
                return new ScopeEntry(ScopeEntryType.Boolean, true);

            bool equals = false;

            if (leftres.Value != null && rightres.Value != null)
            {
                // Check Numbers
                if (Numeric.Contains(leftres.DataType) && Numeric.Contains(rightres.DataType))
                {
                    if (leftres.DataType == ScopeEntryType.Decimal || rightres.DataType == ScopeEntryType.Decimal)
                        equals = leftres.DecimalValue == rightres.DecimalValue;
                    else if (leftres.DataType == ScopeEntryType.Double || rightres.DataType == ScopeEntryType.Double)
                        equals = leftres.DoubleValue == rightres.DoubleValue;
                    else
                        equals = leftres.IntValue == rightres.IntValue;
                }
                else if (leftres.DataType == rightres.DataType)
                {
                    equals = leftres.Value.Equals(rightres.Value);
                }
            }

            switch (binary.Operator.Type)
            {
                case TokenType.Equal:
                    return new ScopeEntry(ScopeEntryType.Boolean, equals);
                case TokenType.NotEqual:
                    return new ScopeEntry(ScopeEntryType.Boolean, !equals);
            }
            throw new AstWalkerException($"Unhandled operator {binary.Operator.Type}");
        }

        private ScopeEntry ExecComparisonOperator(AstEvaluator evaluator,  AstBinaryNode binary)
        {
            ScopeEntry leftres = binary.Left.Exec(evaluator);
            ScopeEntry rightres = binary.Right.Exec(evaluator);

            switch (binary.Operator.Type)
            {
                case TokenType.GreatThan:
                    if (leftres.DataType == ScopeEntryType.Decimal || rightres.DataType == ScopeEntryType.Decimal)
                        return new ScopeEntry(ScopeEntryType.Boolean, leftres.DecimalValue > rightres.DecimalValue);

                    if (leftres.DataType == ScopeEntryType.Double || rightres.DataType == ScopeEntryType.Double)
                        return new ScopeEntry(ScopeEntryType.Boolean, leftres.DoubleValue > rightres.DoubleValue);

                    if ((leftres.DataType == ScopeEntryType.Integer || rightres.DataType == ScopeEntryType.Integer))
                        return new ScopeEntry(ScopeEntryType.Boolean, leftres.IntValue > rightres.IntValue);

                    if (leftres.DataType == ScopeEntryType.String && rightres.DataType == ScopeEntryType.String)
                        return new ScopeEntry(ScopeEntryType.Boolean, leftres.StrValue.CompareTo(rightres.StrValue) > 0);
                    break;

                case TokenType.GreatThanEqual:
                    if (leftres.DataType == ScopeEntryType.Decimal || rightres.DataType == ScopeEntryType.Decimal)
                        return new ScopeEntry(ScopeEntryType.Boolean, leftres.DecimalValue >= rightres.DecimalValue);

                    if (leftres.DataType == ScopeEntryType.Double || rightres.DataType == ScopeEntryType.Double)
                        return new ScopeEntry(ScopeEntryType.Boolean, leftres.DoubleValue >= rightres.DoubleValue);

                    if ((leftres.DataType == ScopeEntryType.Integer || rightres.DataType == ScopeEntryType.Integer))
                        return new ScopeEntry(ScopeEntryType.Boolean, leftres.IntValue >= rightres.IntValue);

                    if (leftres.DataType == ScopeEntryType.String && rightres.DataType == ScopeEntryType.String)
                        return new ScopeEntry(ScopeEntryType.Boolean, leftres.StrValue.CompareTo(rightres.StrValue) >= 0);
                    break;

                case TokenType.LessThan:
                    if (leftres.DataType == ScopeEntryType.Decimal || rightres.DataType == ScopeEntryType.Decimal)
                        return new ScopeEntry(ScopeEntryType.Boolean, leftres.DecimalValue < rightres.DecimalValue);

                    if (leftres.DataType == ScopeEntryType.Double || rightres.DataType == ScopeEntryType.Double)
                        return new ScopeEntry(ScopeEntryType.Boolean, leftres.DoubleValue < rightres.DoubleValue);

                    if ((leftres.DataType == ScopeEntryType.Integer || rightres.DataType == ScopeEntryType.Integer))
                        return new ScopeEntry(ScopeEntryType.Boolean, leftres.IntValue < rightres.IntValue);

                    if (leftres.DataType == ScopeEntryType.String && rightres.DataType == ScopeEntryType.String)
                        return new ScopeEntry(ScopeEntryType.Boolean, leftres.StrValue.CompareTo(rightres.StrValue) < 0);
                    break;

                case TokenType.LessThanEqual:
                    if (leftres.DataType == ScopeEntryType.Decimal || rightres.DataType == ScopeEntryType.Decimal)
                        return new ScopeEntry(ScopeEntryType.Boolean, leftres.DecimalValue <= rightres.DecimalValue);

                    if (leftres.DataType == ScopeEntryType.Double || rightres.DataType == ScopeEntryType.Double)
                        return new ScopeEntry(ScopeEntryType.Boolean, leftres.DoubleValue <= rightres.DoubleValue);

                    if ((leftres.DataType == ScopeEntryType.Integer || rightres.DataType == ScopeEntryType.Integer))
                        return new ScopeEntry(ScopeEntryType.Boolean, leftres.IntValue <= rightres.IntValue);

                    if (leftres.DataType == ScopeEntryType.String && rightres.DataType == ScopeEntryType.String)
                        return new ScopeEntry(ScopeEntryType.Boolean, leftres.StrValue.CompareTo(rightres.StrValue) <= 0);
                    break;
                default:
                    throw new AstWalkerException($"Unhandled operator {binary.Operator.Type}");
            }
            throw new AstWalkerException($"Operator '{binary.Operator.Type}' cannot be applied to operands of type '{leftres.DataType}' and '{rightres.DataType}'");
        }

        private ScopeEntry ExecLogicalOperator(AstEvaluator evaluator,  AstBinaryNode binary)
        {
            ScopeEntry leftres = binary.Left.Exec(evaluator);
            ScopeEntry rightres = binary.Right.Exec(evaluator);

            bool l = !leftres.IsBool ? leftres.Value != null : leftres.BoolValue;
            bool r = !rightres.IsBool ? rightres.Value != null : rightres.BoolValue;

            switch (binary.Operator.Type)
            {
                case TokenType.And:
                    return new ScopeEntry(ScopeEntryType.Boolean, l && r);
                case TokenType.Or:
                    return new ScopeEntry(ScopeEntryType.Boolean, l || r);
            }
            throw new AstWalkerException($"Unhandled operator {binary.Operator.Type}");
        }
    }
}
