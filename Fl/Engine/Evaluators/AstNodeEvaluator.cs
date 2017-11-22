// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Evaluators
{
    public class AstNodeEvaluator : INodeEvaluator<AstEvaluator, AstNode, FlObject>
    {
        public FlObject Evaluate(AstEvaluator evaluator, AstNode node)
        {
            return node.Exec(evaluator);
        }
    }
}
