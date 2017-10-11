// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Evaluators
{
    class VariableNodeEvaluator : INodeEvaluator<AstEvaluator, AstVariableNode, ScopeEntry>
    {
        public ScopeEntry Evaluate(AstEvaluator evaluator, AstVariableNode vardecl)
        {
            string varname = vardecl.Identifier.Value.ToString();
            ScopeEntry init = vardecl.Initializer != null ? vardecl.Initializer.Exec(evaluator) : new ScopeEntry(ScopeEntryType.Null, null);
            evaluator.CurrentScope.NewSymbol(varname, init);
            return init;
        }
    }
}
