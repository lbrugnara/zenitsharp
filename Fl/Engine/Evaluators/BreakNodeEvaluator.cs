// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Engine.Symbols;
using Fl.Parser.Ast;

namespace Fl.Engine.Evaluators
{
    class BreakNodeEvaluator : INodeEvaluator<AstEvaluator, AstBreakNode, FlObject>
    {
        public FlObject Evaluate(AstEvaluator evaluator, AstBreakNode wnode)
        {
            FlObject breakval = wnode.Number?.Exec(evaluator) ?? new FlObject(ObjectType.Integer, 1);
            if (!breakval.IsInt)
                throw new AstWalkerException($"Break expression only allows integer values");
            evaluator.Symtable.SetBreak(breakval.AsInt);
            return breakval;
        }
    }
}
