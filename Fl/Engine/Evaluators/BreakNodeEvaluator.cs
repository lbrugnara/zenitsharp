// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Types;
using Fl.Parser.Ast;

namespace Fl.Engine.Evaluators
{
    class BreakNodeEvaluator : INodeEvaluator<AstEvaluator, AstBreakNode, FlObject>
    {
        public FlObject Evaluate(AstEvaluator evaluator, AstBreakNode wnode)
        {
            FlObject breakval = wnode.Number?.Exec(evaluator) ?? new FlInteger(1);
            if (breakval.ObjectType != IntegerType.Value)
                throw new AstWalkerException($"Break expression only allows integer values");
            evaluator.Symtable.SetBreak(breakval as FlInteger);
            return breakval;
        }
    }
}
