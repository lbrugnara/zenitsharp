// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Evaluators
{
    class AssignmentNodeEvaluator : INodeEvaluator<AstEvaluator, AstAssignmentNode, Symbol>
    {
        public Symbol Evaluate(AstEvaluator walker, AstAssignmentNode node)
        {
            string idname = node.Identifier.Value.ToString();
            Symbol symbol = walker.CurrentScope[idname];
            if (symbol == null)
                throw new AstWalkerException($"Symbol '{idname}' does not exist in the current context");
            if (symbol.Storage == StorageType.Constant)
            {
                if (symbol.DataType == SymbolType.Function)
                    throw new AstWalkerException($"Left-hand side of an assignment must be a variable. '{idname}' is a function");
                else
                    throw new AstWalkerException($"Left-hand side of an assignment must be a variable. '{idname}' is a constant value");
            }
            walker.CurrentScope.UpdateSymbol(idname, (symbol = node.Expression.Exec(walker)));
            return symbol;
        }
    }
}
