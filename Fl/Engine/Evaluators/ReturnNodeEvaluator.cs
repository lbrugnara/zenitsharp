// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Parser.Ast;

namespace Fl.Engine.Evaluators
{
    class ReturnNodeEvaluator : INodeEvaluator<AstEvaluator, AstReturnNode, ScopeEntry>
    {
        public ScopeEntry Evaluate(AstEvaluator evaluator, AstReturnNode wnode)
        {
            ScopeEntry retval = wnode.Expression?.Exec(evaluator) ?? new ScopeEntry(ScopeEntryType.Null, null);
            evaluator.CurrentScope.ReturnValue = retval;
            return retval;
        }
    }
}
