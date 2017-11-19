// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Evaluators
{
    class AssignmentNodeEvaluator : INodeEvaluator<AstEvaluator, AstAssignmentNode, FlObject>
    {
        public FlObject Evaluate(AstEvaluator walker, AstAssignmentNode node)
        {
            string idname = node.Identifier.Value.ToString();
            Symbol symbol = walker.Symtable.GetSymbol(idname);

            if (symbol.Storage == StorageType.Constant)
            {
                if (symbol.Type == ObjectType.Function)
                    throw new AstWalkerException($"Left-hand side of an assignment must be a variable. '{idname}' is a function");
                if (symbol.Type == ObjectType.Namespace)
                    throw new AstWalkerException($"Left-hand side of an assignment must be a variable. '{idname}' is a namespace");
                else
                    throw new AstWalkerException($"Left-hand side of an assignment must be a variable. '{idname}' is a constant value");
            }

            var assignmentResult = node.Expression.Exec(walker);
            if (node.AssignmentOp.Type != TokenType.Assignment)
            {
                if ((!symbol.Type.IsNumeric() || !assignmentResult.Type.IsNumeric()) && (symbol.Type != assignmentResult.Type || symbol.Type != ObjectType.String))
                    throw new AstWalkerException($"Operator '{node.AssignmentOp.Value}' cannot be applied to operands of type '{symbol.Type}' and '{assignmentResult.Type}'");
            }

            switch (node.AssignmentOp.Type)
            {                
                case TokenType.IncrementAndAssign:
                    return IncrementAndAssing(node.AssignmentOp, symbol, assignmentResult);
                case TokenType.DecrementAndAssign:
                    return DecrementAndAssign(node.AssignmentOp, symbol, assignmentResult);
                case TokenType.MultAndAssign:
                    return MultAndAssign(node.AssignmentOp, symbol, assignmentResult);
                case TokenType.DivideAndAssign:
                    return DivideAndAssign(node.AssignmentOp, symbol, assignmentResult);
                case TokenType.Assignment:
                    walker.Symtable.UpdateSymbol(idname, new FlObject(assignmentResult.Type, assignmentResult.Value));
                    return new FlObject(assignmentResult.Type, assignmentResult.Value);
            }
            throw new AstWalkerException($"Unrecognized assignment token {node.AssignmentOp.Type}");
        }

        private FlObject IncrementAndAssing(Token assign, Symbol symbol, FlObject result)
        {
            switch (result.Type)
            {
                case ObjectType.Integer:
                    symbol.Binding.Value = symbol.Binding.AsInt + result.AsInt; 
                    break;
                case ObjectType.Double:
                    symbol.Binding.Value = symbol.Binding.AsDouble + result.AsDouble;
                    break;
                case ObjectType.Decimal:
                    symbol.Binding.Value = symbol.Binding.AsDecimal + result.AsDecimal;
                    break;
                case ObjectType.String:
                    symbol.Binding.Value = symbol.Binding.AsString + result.AsString;
                    break;
                default:
                    throw new AstWalkerException($"Operator '{assign.Value}' cannot be applied to operand of type {result.Type}");
            }
            return new FlObject(symbol.Type, symbol.Binding.Value);
        }

        private FlObject DecrementAndAssign(Token assign, Symbol symbol, FlObject result)
        {
            switch (result.Type)
            {
                case ObjectType.Integer:
                    symbol.Binding.Value = symbol.Binding.AsInt - result.AsInt;
                    break;
                case ObjectType.Double:
                    symbol.Binding.Value = symbol.Binding.AsDouble - result.AsDouble;
                    break;
                case ObjectType.Decimal:
                    symbol.Binding.Value = symbol.Binding.AsDecimal - result.AsDecimal;
                    break;
                default:
                    throw new AstWalkerException($"Operator '{assign.Value}' cannot be applied to operand of type {result.Type}");
            }
            return new FlObject(symbol.Type, symbol.Binding.Value);
        }

        private FlObject MultAndAssign(Token assign, Symbol symbol, FlObject result)
        {
            switch (result.Type)
            {
                case ObjectType.Integer:
                    symbol.Binding.Value = symbol.Binding.AsInt * result.AsInt;
                    break;
                case ObjectType.Double:
                    symbol.Binding.Value = symbol.Binding.AsDouble * result.AsDouble;
                    break;
                case ObjectType.Decimal:
                    symbol.Binding.Value = symbol.Binding.AsDecimal * result.AsDecimal;
                    break;
                default:
                    throw new AstWalkerException($"Operator '{assign.Value}' cannot be applied to operand of type {result.Type}");
            }
            return new FlObject(symbol.Type, symbol.Binding.Value);
        }

        private FlObject DivideAndAssign(Token assign, Symbol symbol, FlObject result)
        {
            switch (result.Type)
            {
                case ObjectType.Integer:
                    symbol.Binding.Value = symbol.Binding.AsInt / result.AsInt;
                    break;
                case ObjectType.Double:
                    symbol.Binding.Value = symbol.Binding.AsDouble / result.AsDouble;
                    break;
                case ObjectType.Decimal:
                    symbol.Binding.Value = symbol.Binding.AsDecimal / result.AsDecimal;
                    break;
                default:
                    throw new AstWalkerException($"Operator '{assign.Value}' cannot be applied to operand of type {result.Type}");
            }
            return new FlObject(symbol.Type, symbol.Binding.Value);
        }
    }
}
