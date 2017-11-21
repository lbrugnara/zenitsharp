// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Engine.Symbols;
using Fl.Parser.Ast;

namespace Fl.Engine.Evaluators
{
    class ReturnNodeEvaluator : INodeEvaluator<AstEvaluator, AstReturnNode, FlObject>
    {
        public FlObject Evaluate(AstEvaluator evaluator, AstReturnNode wnode)
        {
            FlObject retval = wnode.Expression?.Exec(evaluator) ?? FlNull.Value;
            evaluator.Symtable.ReturnValue = retval;
            return retval;
        }
    }
}
