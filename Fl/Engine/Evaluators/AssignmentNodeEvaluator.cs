// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Evaluators
{
    class AssignmentNodeEvaluator : INodeEvaluator<AstEvaluator, AstAssignmentNode, FlObject>
    {
        private static AccessorNodeEvaluator _AccessorEv = new AccessorNodeEvaluator();

        public FlObject Evaluate(AstEvaluator walker, AstAssignmentNode node)
        {
            string idname = node.Identifier?.Value.ToString() ?? node.Accessor?.Self.Value.ToString();
            Symbol symbol = null;
            if (node.Accessor != null)
            {
                symbol = _AccessorEv.GetSymbol(walker, node.Accessor);
            }
            else
            {                
                symbol = walker.Symtable.GetSymbol(idname);
            }

            if (symbol.SymbolType == SymbolType.Constant)
            {
                if (symbol.Binding.ObjectType == FuncType.Value)
                    throw new AstWalkerException($"Left-hand side of an assignment must be a variable. '{idname}' is a function");
                if (symbol.Binding.ObjectType == NamespaceType.Value)
                    throw new AstWalkerException($"Left-hand side of an assignment must be a variable. '{idname}' is a namespace");
                else
                    throw new AstWalkerException($"Left-hand side of an assignment must be a variable. '{idname}' is a constant value");
            }

            var assignmentResult = node.Expression.Exec(walker);
            /*if (node.AssignmentOp.Type != TokenType.Assignment)
            {
                if ((!(symbol.Binding.ObjectType is NumericType) || !(assignmentResult.ObjectType is NumericType)) && (symbol.ObjectType != assignmentResult.ObjectType || symbol.Binding.ObjectType != StringType.Value))
                    throw new AstWalkerException($"Operator '{node.AssignmentOp.Value}' cannot be applied to operands of type '{symbol.ObjectType}' and '{assignmentResult.ObjectType}'");
            }*/

            switch (node.AssignmentOp.Type)
            {                
                case TokenType.IncrementAndAssign:
                    symbol.Binding.AddAndAssign(assignmentResult);
                    return symbol.Binding.Clone();

                case TokenType.DecrementAndAssign:
                    symbol.Binding.SubstractAndAssign(assignmentResult);
                    return symbol.Binding.Clone();

                case TokenType.MultAndAssign:
                    symbol.Binding.MultiplyAndAssing(assignmentResult);
                    return symbol.Binding.Clone();
                case TokenType.DivideAndAssign:
                    symbol.Binding.DivideAndAssing(assignmentResult);
                    return symbol.Binding.Clone();

                case TokenType.Assignment:
                    //walker.Symtable.UpdateSymbol(idname, assignmentResult);
                    if (symbol.Binding == FlNull.Value)
                    {
                        symbol.UpdateBinding(assignmentResult);
                    }
                    else
                    {
                        symbol.Binding.Assign(assignmentResult);
                    }
                    return assignmentResult.Clone();
            }
            throw new AstWalkerException($"Unrecognized assignment token {node.AssignmentOp.Type}");
        }
    }
}
