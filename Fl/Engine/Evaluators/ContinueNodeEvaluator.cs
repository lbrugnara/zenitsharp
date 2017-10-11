// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Parser.Ast;

namespace Fl.Engine.Evaluators
{
    class ContinueNodeEvaluator : INodeEvaluator<AstEvaluator, AstContinueNode, ScopeEntry>
    {
        public ScopeEntry Evaluate(AstEvaluator evaluator, AstContinueNode cnode)
        {
            evaluator.CurrentScope.SetContinue();
            return null;
        }
    }
}
