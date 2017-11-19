// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Engine.Symbols;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Evaluators
{
    class NullCoalescingNodeEvaluator : INodeEvaluator<AstEvaluator, AstNullCoalescingNode, FlObject>
    {
        public FlObject Evaluate(AstEvaluator evaluator, AstNullCoalescingNode nullc)
        {
            FlObject leftResult = nullc.Left.Exec(evaluator);
            if (leftResult.Type == ObjectType.Null)
                return nullc.Right.Exec(evaluator);
            return leftResult;
        }
    }
}
