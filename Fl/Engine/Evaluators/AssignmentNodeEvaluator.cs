// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Evaluators
{
    class AssignmentNodeEvaluator : INodeEvaluator<AstEvaluator, AstAssignmentNode, ScopeEntry>
    {
        public ScopeEntry Evaluate(AstEvaluator walker, AstAssignmentNode node)
        {
            string idname = node.Identifier.Value.ToString();
            ScopeEntry symbol = walker.CurrentScope[idname];
            if (symbol == null)
                throw new AstWalkerException($"Symbol '{idname}' does not exist in the current context");
            if (symbol.Modifier == StorageType.Constant)
                throw new AstWalkerException($"Left-hand side of an assignment must be a variable. '{idname}' is a constant value");
            walker.CurrentScope.UpdateSymbol(idname, (symbol = node.Expression.Exec(walker)));
            return symbol;
        }
    }
}
