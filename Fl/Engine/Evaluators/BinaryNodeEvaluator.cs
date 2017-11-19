// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fl.Engine.Evaluators
{
    class BinaryNodeEvaluator : INodeEvaluator<AstEvaluator, AstBinaryNode, FlObject>
    {
        private static readonly ObjectType[] Numeric = new ObjectType[] { ObjectType.Integer, ObjectType.Double, ObjectType.Decimal };

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

            if (!Numeric.Contains(leftres.Type) || !Numeric.Contains(rightres.Type))
            {
                throw new AstWalkerException($"Operator '{binary.Operator.Type}' cannot be applied to operands of type '{leftres.Type}' and '{rightres.Type}'");
            }

            switch (binary.Operator.Type)
            {
                case TokenType.Multiplication:
                    if (leftres.Type == ObjectType.Decimal || rightres.Type == ObjectType.Decimal)
                        return new FlObject(ObjectType.Decimal, leftres.AsDecimal * rightres.AsDecimal);

                    if (leftres.Type == ObjectType.Double || rightres.Type == ObjectType.Double)
                        return new FlObject(ObjectType.Double, leftres.AsDouble * rightres.AsDouble);

                    return new FlObject(ObjectType.Integer, leftres.AsInt * rightres.AsInt);

                case TokenType.Division:
                    if (leftres.Type == ObjectType.Decimal || rightres.Type == ObjectType.Decimal)
                        return new FlObject(ObjectType.Decimal, leftres.AsDecimal / rightres.AsDecimal);

                    // Integer will be promoted to double
                    return new FlObject(ObjectType.Double, leftres.AsDouble / rightres.AsDouble);
            }
            throw new AstWalkerException($"Unhandled operator {binary.Operator.Type}");
        }

        private FlObject ExecAdditionOperator(AstEvaluator evaluator,  AstBinaryNode binary)
        {
            FlObject leftres = binary.Left.Exec(evaluator);
            FlObject rightres = binary.Right.Exec(evaluator);

            if (!Numeric.Contains(leftres.Type) || !Numeric.Contains(rightres.Type))
            {
                if (leftres.Type != ObjectType.String && rightres.Type != ObjectType.String)
                    throw new AstWalkerException($"Operator '{binary.Operator.Type}' cannot be applied to operands of type '{leftres.Type}' and '{rightres.Type}'");
            }

            switch (binary.Operator.Type)
            {
                case TokenType.Addition:
                    if (leftres.Type == ObjectType.String || rightres.Type == ObjectType.String)
                        return new FlObject(ObjectType.String, leftres.AsString + rightres.AsString);

                    if (leftres.Type == ObjectType.Decimal || rightres.Type == ObjectType.Decimal)
                        return new FlObject(ObjectType.Decimal, leftres.AsDecimal + rightres.AsDecimal);

                    if (leftres.Type == ObjectType.Double || rightres.Type == ObjectType.Double)
                        return new FlObject(ObjectType.Double, leftres.AsDouble + rightres.AsDouble);

                    return new FlObject(ObjectType.Integer, leftres.AsInt + rightres.AsInt);
                case TokenType.Minus:
                    if (leftres.Type == ObjectType.Decimal || rightres.Type == ObjectType.Decimal)
                        return new FlObject(ObjectType.Decimal, leftres.AsDecimal - rightres.AsDecimal);

                    if (leftres.Type == ObjectType.Double || rightres.Type == ObjectType.Double)
                        return new FlObject(ObjectType.Double, leftres.AsDouble - rightres.AsDouble);

                    return new FlObject(ObjectType.Integer, leftres.AsInt - rightres.AsInt);
            }
            throw new AstWalkerException($"Unhandled operator {binary.Operator.Type}");
        }

        private FlObject ExecEqualityOperator(AstEvaluator evaluator,  AstBinaryNode binary)
        {
            FlObject leftres = binary.Left.Exec(evaluator);
            FlObject rightres = binary.Right.Exec(evaluator);

            if (leftres.IsNull && rightres.IsNull)
                return new FlObject(ObjectType.Boolean, true);

            bool equals = false;

            if (leftres.Value != null && rightres.Value != null)
            {
                // Check Numbers
                if (Numeric.Contains(leftres.Type) && Numeric.Contains(rightres.Type))
                {
                    if (leftres.Type == ObjectType.Decimal || rightres.Type == ObjectType.Decimal)
                        equals = leftres.AsDecimal == rightres.AsDecimal;
                    else if (leftres.Type == ObjectType.Double || rightres.Type == ObjectType.Double)
                        equals = leftres.AsDouble == rightres.AsDouble;
                    else
                        equals = leftres.AsInt == rightres.AsInt;
                }
                else if (leftres.Type == rightres.Type)
                {
                    equals = leftres.Value.Equals(rightres.Value);
                }
            }

            switch (binary.Operator.Type)
            {
                case TokenType.Equal:
                    return new FlObject(ObjectType.Boolean, equals);
                case TokenType.NotEqual:
                    return new FlObject(ObjectType.Boolean, !equals);
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
                    if (leftres.Type == ObjectType.Decimal || rightres.Type == ObjectType.Decimal)
                        return new FlObject(ObjectType.Boolean, leftres.AsDecimal > rightres.AsDecimal);

                    if (leftres.Type == ObjectType.Double || rightres.Type == ObjectType.Double)
                        return new FlObject(ObjectType.Boolean, leftres.AsDouble > rightres.AsDouble);

                    if ((leftres.Type == ObjectType.Integer || rightres.Type == ObjectType.Integer))
                        return new FlObject(ObjectType.Boolean, leftres.AsInt > rightres.AsInt);

                    if (leftres.Type == ObjectType.String && rightres.Type == ObjectType.String)
                        return new FlObject(ObjectType.Boolean, leftres.AsString.CompareTo(rightres.AsString) > 0);
                    break;

                case TokenType.GreatThanEqual:
                    if (leftres.Type == ObjectType.Decimal || rightres.Type == ObjectType.Decimal)
                        return new FlObject(ObjectType.Boolean, leftres.AsDecimal >= rightres.AsDecimal);

                    if (leftres.Type == ObjectType.Double || rightres.Type == ObjectType.Double)
                        return new FlObject(ObjectType.Boolean, leftres.AsDouble >= rightres.AsDouble);

                    if ((leftres.Type == ObjectType.Integer || rightres.Type == ObjectType.Integer))
                        return new FlObject(ObjectType.Boolean, leftres.AsInt >= rightres.AsInt);

                    if (leftres.Type == ObjectType.String && rightres.Type == ObjectType.String)
                        return new FlObject(ObjectType.Boolean, leftres.AsString.CompareTo(rightres.AsString) >= 0);
                    break;

                case TokenType.LessThan:
                    if (leftres.Type == ObjectType.Decimal || rightres.Type == ObjectType.Decimal)
                        return new FlObject(ObjectType.Boolean, leftres.AsDecimal < rightres.AsDecimal);

                    if (leftres.Type == ObjectType.Double || rightres.Type == ObjectType.Double)
                        return new FlObject(ObjectType.Boolean, leftres.AsDouble < rightres.AsDouble);

                    if ((leftres.Type == ObjectType.Integer || rightres.Type == ObjectType.Integer))
                        return new FlObject(ObjectType.Boolean, leftres.AsInt < rightres.AsInt);

                    if (leftres.Type == ObjectType.String && rightres.Type == ObjectType.String)
                        return new FlObject(ObjectType.Boolean, leftres.AsString.CompareTo(rightres.AsString) < 0);
                    break;

                case TokenType.LessThanEqual:
                    if (leftres.Type == ObjectType.Decimal || rightres.Type == ObjectType.Decimal)
                        return new FlObject(ObjectType.Boolean, leftres.AsDecimal <= rightres.AsDecimal);

                    if (leftres.Type == ObjectType.Double || rightres.Type == ObjectType.Double)
                        return new FlObject(ObjectType.Boolean, leftres.AsDouble <= rightres.AsDouble);

                    if ((leftres.Type == ObjectType.Integer || rightres.Type == ObjectType.Integer))
                        return new FlObject(ObjectType.Boolean, leftres.AsInt <= rightres.AsInt);

                    if (leftres.Type == ObjectType.String && rightres.Type == ObjectType.String)
                        return new FlObject(ObjectType.Boolean, leftres.AsString.CompareTo(rightres.AsString) <= 0);
                    break;
                default:
                    throw new AstWalkerException($"Unhandled operator {binary.Operator.Type}");
            }
            throw new AstWalkerException($"Operator '{binary.Operator.Type}' cannot be applied to operands of type '{leftres.Type}' and '{rightres.Type}'");
        }

        private FlObject ExecLogicalOperator(AstEvaluator evaluator,  AstBinaryNode binary)
        {
            FlObject leftres = binary.Left.Exec(evaluator);
            FlObject rightres = binary.Right.Exec(evaluator);

            bool l = !leftres.IsBool ? leftres.Value != null : leftres.AsBool;
            bool r = !rightres.IsBool ? rightres.Value != null : rightres.AsBool;

            switch (binary.Operator.Type)
            {
                case TokenType.And:
                    return new FlObject(ObjectType.Boolean, l && r);
                case TokenType.Or:
                    return new FlObject(ObjectType.Boolean, l || r);
            }
            throw new AstWalkerException($"Unhandled operator {binary.Operator.Type}");
        }
    }
}
