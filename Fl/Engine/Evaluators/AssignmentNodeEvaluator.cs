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
            Symbol symbol = null;
            if (node.Accessor != null)
            {
                symbol = _AccessorEv.GetSymbol(walker, node.Accessor);
            }
            else if (node.Lvalues != null)
            {
                var rvalue = node.Expression.Exec(walker) as FlTuple;
                var rcount = rvalue.Value.Count;
                var lvalue = node.Lvalues.Exec(walker) as FlTuple;
                var lcount = lvalue.Value.Count;
                for (int i=0; i < lcount; i++)
                {
                    var s = _AccessorEv.GetSymbol(walker, node.Lvalues.Items[i] as AstAccessorNode);
                    if (i >= rcount)
                        break;

                    s.UpdateBinding(rvalue.Value[i]);
                    lvalue.Value[i] = s.Binding;
                }
                return lvalue;
            }
            else
            {
                string idname = node.Accessor?.Self.Value.ToString();
                symbol = walker.Symtable.GetSymbol(idname);
            }

            if (symbol.SymbolType == SymbolType.Constant)
            {
                if (symbol.Binding.ObjectType == FuncType.Value)
                    throw new AstWalkerException($"Left-hand side of an assignment must be a variable. '{symbol.Name}' is a function");
                if (symbol.Binding.ObjectType == NamespaceType.Value)
                    throw new AstWalkerException($"Left-hand side of an assignment must be a variable. '{symbol.Name}' is a namespace");
                else
                    throw new AstWalkerException($"Left-hand side of an assignment must be a variable. '{symbol.Name}' is a constant value");
            }

            var assignmentResult = node.Expression.Exec(walker);
            switch (node.AssignmentOp.Type)
            {                
                case TokenType.IncrementAndAssign:
                    symbol.Binding.AddAndAssign(assignmentResult);
                    return symbol.Binding.Clone();

                case TokenType.DecrementAndAssign:
                    symbol.Binding.SubtractAndAssign(assignmentResult);
                    return symbol.Binding.Clone();

                case TokenType.MultAndAssign:
                    symbol.Binding.MultiplyAndAssing(assignmentResult);
                    return symbol.Binding.Clone();
                case TokenType.DivideAndAssign:
                    symbol.Binding.DivideAndAssing(assignmentResult);
                    return symbol.Binding.Clone();

                case TokenType.Assignment:
                    if (symbol.Binding == FlNull.Value)
                    {
                        symbol.UpdateBinding(assignmentResult.IsPrimitive ? assignmentResult.Clone() : assignmentResult);
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
