// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;
using Fl.Parser;
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
            if (node is AstDestructuringAssignmentNode)
                return MakeDestructuringAssignment(node as AstDestructuringAssignmentNode, walker);

            if (node is AstVariableAssignmentNode)
                return MakeVariableAssignment(node as AstVariableAssignmentNode, walker);

            throw new AstWalkerException($"Unhandled node type {node.GetType().FullName}");
        }

        private FlObject MakeVariableAssignment(AstVariableAssignmentNode node, AstEvaluator walker)
        {
            // Get the symbol reference by this accessor. This call will raise an exception if the symbol is not defined
            Symbol symbol = _AccessorEv.GetSymbol(walker, (node as AstVariableAssignmentNode).Accessor);

            // Check the SymbolType to see if it's a valid lvalue
            if (symbol.SymbolType == SymbolType.Constant)
            {
                if (symbol.Binding.ObjectType == FuncType.Value)
                    throw new AstWalkerException($"Left-hand side of an assignment must be a variable. '{symbol.Name}' is a function");

                if (symbol.Binding.ObjectType == NamespaceType.Value)
                    throw new AstWalkerException($"Left-hand side of an assignment must be a variable. '{symbol.Name}' is a namespace");

                throw new AstWalkerException($"Left-hand side of an assignment must be a variable. '{symbol.Name}' is a constant value");
            }

            // Evaluate the right-hand expression to get the rvalue
            var assignmentResult = node.Expression.Exec(walker);

            // Check the assignment type and call specific FlOBject method
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
                        // Primitive values are passed by value
                        symbol.UpdateBinding(assignmentResult.IsPrimitive ? assignmentResult.Clone() : assignmentResult);
                    }
                    else
                    {
                        // If the FlObject already has a value (is not FlNull), let the Assign method handle each object's specific operator =
                        if (symbol.Binding.IsPrimitive)
                            symbol.UpdateBinding(assignmentResult.IsPrimitive ? assignmentResult.Clone() : assignmentResult);
                        else
                            symbol.Binding.Assign(assignmentResult);
                    }
                    return assignmentResult.Clone();
            }
            throw new AstWalkerException($"Unrecognized assignment token {node.AssignmentOp.Type}");
        }

        private FlObject MakeDestructuringAssignment(AstDestructuringAssignmentNode destructuring, AstEvaluator walker)
        {
            var rnode = destructuring.Expression.Exec(walker);
            if (!(rnode is FlTuple))
            {
                // TODO: Here we can handle the destructuring for specific objects
                throw new ParserException($"Cannot destructure object of type '{rnode.ObjectType}'");
            }

            // Get the count of elements in the right-hand side
            var rvalue = rnode as FlTuple;
            var rcount = rvalue.Value.Count;

            // Process the left-hand side to get a reference to an FlTuple
            var lvalue = destructuring.Lvalues.Exec(walker) as FlTuple;
            var lcount = lvalue.Value.Count;

            for (int i = 0; i < lcount; i++)
            {
                // Get one of the nodes in the left-hand side 
                // If it is not an accessor node, ignore this element (it could be an assignment of type (a,,c) = (1,2,3)
                var accessor = destructuring.Lvalues.Items[i] as AstAccessorNode;
                if (accessor == null)
                    continue;

                // Get the symbol
                var s = _AccessorEv.GetSymbol(walker, accessor);

                // Break if there are more left-hand values than right-hand
                if (i >= rcount)
                    break;

                // Update the symbol referenced by the accessor and update the lvalue
                s.UpdateBinding(rvalue.Value[i]);
                lvalue.Value[i] = s.Binding;
            }
            return rvalue;
        }
    }
}
