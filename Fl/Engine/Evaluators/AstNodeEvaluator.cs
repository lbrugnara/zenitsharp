// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Evaluators
{
    public class AstNodeEvaluator : INodeEvaluator<AstEvaluator, AstNode, ScopeEntry>
    {
        public ScopeEntry Evaluate(AstEvaluator evaluator, AstNode node)
        {
            return node.Exec(evaluator);
        }
    }
}
