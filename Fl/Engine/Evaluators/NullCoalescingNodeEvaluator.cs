// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Evaluators
{
    class NullCoalescingNodeEvaluator : INodeVisitor<AstEvaluator, AstNullCoalescingNode, FlObject>
    {
        public FlObject Visit(AstEvaluator evaluator, AstNullCoalescingNode nullc)
        {
            FlObject leftResult = nullc.Left.Exec(evaluator);
            if (leftResult.ObjectType == NullType.Value)
                return nullc.Right.Exec(evaluator);
            return leftResult;
        }
    }
}
