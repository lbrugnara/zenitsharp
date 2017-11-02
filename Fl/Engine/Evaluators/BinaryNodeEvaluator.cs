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
    class BinaryNodeEvaluator : INodeEvaluator<AstEvaluator, AstBinaryNode, Symbol>
    {
        private static readonly SymbolType[] Numeric = new SymbolType[] { SymbolType.Integer, SymbolType.Double, SymbolType.Decimal };

        public Symbol Evaluate(AstEvaluator evaluator, AstBinaryNode binary)
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

        private Symbol ExecMultOperator(AstEvaluator evaluator,  AstBinaryNode binary)
        {
            Symbol leftres = binary.Left.Exec(evaluator);
            Symbol rightres = binary.Right.Exec(evaluator);

            if (!Numeric.Contains(leftres.DataType) || !Numeric.Contains(rightres.DataType))
            {
                throw new AstWalkerException($"Operator '{binary.Operator.Type}' cannot be applied to operands of type '{leftres.DataType}' and '{rightres.DataType}'");
            }

            switch (binary.Operator.Type)
            {
                case TokenType.Multiplication:
                    if (leftres.DataType == SymbolType.Decimal || rightres.DataType == SymbolType.Decimal)
                        return new Symbol(SymbolType.Decimal, leftres.AsDecimal * rightres.AsDecimal);

                    if (leftres.DataType == SymbolType.Double || rightres.DataType == SymbolType.Double)
                        return new Symbol(SymbolType.Double, leftres.AsDouble * rightres.AsDouble);

                    return new Symbol(SymbolType.Integer, leftres.AsInt * rightres.AsInt);

                case TokenType.Division:
                    if (leftres.DataType == SymbolType.Decimal || rightres.DataType == SymbolType.Decimal)
                        return new Symbol(SymbolType.Decimal, leftres.AsDecimal / rightres.AsDecimal);

                    // Integer will be promoted to double
                    return new Symbol(SymbolType.Double, leftres.AsDouble / rightres.AsDouble);
            }
            throw new AstWalkerException($"Unhandled operator {binary.Operator.Type}");
        }

        private Symbol ExecAdditionOperator(AstEvaluator evaluator,  AstBinaryNode binary)
        {
            Symbol leftres = binary.Left.Exec(evaluator);
            Symbol rightres = binary.Right.Exec(evaluator);

            if (!Numeric.Contains(leftres.DataType) || !Numeric.Contains(rightres.DataType))
            {
                if (leftres.DataType != SymbolType.String && rightres.DataType != SymbolType.String)
                    throw new AstWalkerException($"Operator '{binary.Operator.Type}' cannot be applied to operands of type '{leftres.DataType}' and '{rightres.DataType}'");
            }

            switch (binary.Operator.Type)
            {
                case TokenType.Addition:
                    if (leftres.DataType == SymbolType.String || rightres.DataType == SymbolType.String)
                        return new Symbol(SymbolType.String, leftres.AsString + rightres.AsString);

                    if (leftres.DataType == SymbolType.Decimal || rightres.DataType == SymbolType.Decimal)
                        return new Symbol(SymbolType.Decimal, leftres.AsDecimal + rightres.AsDecimal);

                    if (leftres.DataType == SymbolType.Double || rightres.DataType == SymbolType.Double)
                        return new Symbol(SymbolType.Double, leftres.AsDouble + rightres.AsDouble);

                    return new Symbol(SymbolType.Integer, leftres.AsInt + rightres.AsInt);
                case TokenType.Minus:
                    if (leftres.DataType == SymbolType.Decimal || rightres.DataType == SymbolType.Decimal)
                        return new Symbol(SymbolType.Decimal, leftres.AsDecimal - rightres.AsDecimal);

                    if (leftres.DataType == SymbolType.Double || rightres.DataType == SymbolType.Double)
                        return new Symbol(SymbolType.Double, leftres.AsDouble - rightres.AsDouble);

                    return new Symbol(SymbolType.Integer, leftres.AsInt - rightres.AsInt);
            }
            throw new AstWalkerException($"Unhandled operator {binary.Operator.Type}");
        }

        private Symbol ExecEqualityOperator(AstEvaluator evaluator,  AstBinaryNode binary)
        {
            Symbol leftres = binary.Left.Exec(evaluator);
            Symbol rightres = binary.Right.Exec(evaluator);

            if (leftres.IsNull && rightres.IsNull)
                return new Symbol(SymbolType.Boolean, true);

            bool equals = false;

            if (leftres.Value != null && rightres.Value != null)
            {
                // Check Numbers
                if (Numeric.Contains(leftres.DataType) && Numeric.Contains(rightres.DataType))
                {
                    if (leftres.DataType == SymbolType.Decimal || rightres.DataType == SymbolType.Decimal)
                        equals = leftres.AsDecimal == rightres.AsDecimal;
                    else if (leftres.DataType == SymbolType.Double || rightres.DataType == SymbolType.Double)
                        equals = leftres.AsDouble == rightres.AsDouble;
                    else
                        equals = leftres.AsInt == rightres.AsInt;
                }
                else if (leftres.DataType == rightres.DataType)
                {
                    equals = leftres.Value.Equals(rightres.Value);
                }
            }

            switch (binary.Operator.Type)
            {
                case TokenType.Equal:
                    return new Symbol(SymbolType.Boolean, equals);
                case TokenType.NotEqual:
                    return new Symbol(SymbolType.Boolean, !equals);
            }
            throw new AstWalkerException($"Unhandled operator {binary.Operator.Type}");
        }

        private Symbol ExecComparisonOperator(AstEvaluator evaluator,  AstBinaryNode binary)
        {
            Symbol leftres = binary.Left.Exec(evaluator);
            Symbol rightres = binary.Right.Exec(evaluator);

            switch (binary.Operator.Type)
            {
                case TokenType.GreatThan:
                    if (leftres.DataType == SymbolType.Decimal || rightres.DataType == SymbolType.Decimal)
                        return new Symbol(SymbolType.Boolean, leftres.AsDecimal > rightres.AsDecimal);

                    if (leftres.DataType == SymbolType.Double || rightres.DataType == SymbolType.Double)
                        return new Symbol(SymbolType.Boolean, leftres.AsDouble > rightres.AsDouble);

                    if ((leftres.DataType == SymbolType.Integer || rightres.DataType == SymbolType.Integer))
                        return new Symbol(SymbolType.Boolean, leftres.AsInt > rightres.AsInt);

                    if (leftres.DataType == SymbolType.String && rightres.DataType == SymbolType.String)
                        return new Symbol(SymbolType.Boolean, leftres.AsString.CompareTo(rightres.AsString) > 0);
                    break;

                case TokenType.GreatThanEqual:
                    if (leftres.DataType == SymbolType.Decimal || rightres.DataType == SymbolType.Decimal)
                        return new Symbol(SymbolType.Boolean, leftres.AsDecimal >= rightres.AsDecimal);

                    if (leftres.DataType == SymbolType.Double || rightres.DataType == SymbolType.Double)
                        return new Symbol(SymbolType.Boolean, leftres.AsDouble >= rightres.AsDouble);

                    if ((leftres.DataType == SymbolType.Integer || rightres.DataType == SymbolType.Integer))
                        return new Symbol(SymbolType.Boolean, leftres.AsInt >= rightres.AsInt);

                    if (leftres.DataType == SymbolType.String && rightres.DataType == SymbolType.String)
                        return new Symbol(SymbolType.Boolean, leftres.AsString.CompareTo(rightres.AsString) >= 0);
                    break;

                case TokenType.LessThan:
                    if (leftres.DataType == SymbolType.Decimal || rightres.DataType == SymbolType.Decimal)
                        return new Symbol(SymbolType.Boolean, leftres.AsDecimal < rightres.AsDecimal);

                    if (leftres.DataType == SymbolType.Double || rightres.DataType == SymbolType.Double)
                        return new Symbol(SymbolType.Boolean, leftres.AsDouble < rightres.AsDouble);

                    if ((leftres.DataType == SymbolType.Integer || rightres.DataType == SymbolType.Integer))
                        return new Symbol(SymbolType.Boolean, leftres.AsInt < rightres.AsInt);

                    if (leftres.DataType == SymbolType.String && rightres.DataType == SymbolType.String)
                        return new Symbol(SymbolType.Boolean, leftres.AsString.CompareTo(rightres.AsString) < 0);
                    break;

                case TokenType.LessThanEqual:
                    if (leftres.DataType == SymbolType.Decimal || rightres.DataType == SymbolType.Decimal)
                        return new Symbol(SymbolType.Boolean, leftres.AsDecimal <= rightres.AsDecimal);

                    if (leftres.DataType == SymbolType.Double || rightres.DataType == SymbolType.Double)
                        return new Symbol(SymbolType.Boolean, leftres.AsDouble <= rightres.AsDouble);

                    if ((leftres.DataType == SymbolType.Integer || rightres.DataType == SymbolType.Integer))
                        return new Symbol(SymbolType.Boolean, leftres.AsInt <= rightres.AsInt);

                    if (leftres.DataType == SymbolType.String && rightres.DataType == SymbolType.String)
                        return new Symbol(SymbolType.Boolean, leftres.AsString.CompareTo(rightres.AsString) <= 0);
                    break;
                default:
                    throw new AstWalkerException($"Unhandled operator {binary.Operator.Type}");
            }
            throw new AstWalkerException($"Operator '{binary.Operator.Type}' cannot be applied to operands of type '{leftres.DataType}' and '{rightres.DataType}'");
        }

        private Symbol ExecLogicalOperator(AstEvaluator evaluator,  AstBinaryNode binary)
        {
            Symbol leftres = binary.Left.Exec(evaluator);
            Symbol rightres = binary.Right.Exec(evaluator);

            bool l = !leftres.IsBool ? leftres.Value != null : leftres.AsBool;
            bool r = !rightres.IsBool ? rightres.Value != null : rightres.AsBool;

            switch (binary.Operator.Type)
            {
                case TokenType.And:
                    return new Symbol(SymbolType.Boolean, l && r);
                case TokenType.Or:
                    return new Symbol(SymbolType.Boolean, l || r);
            }
            throw new AstWalkerException($"Unhandled operator {binary.Operator.Type}");
        }
    }
}
