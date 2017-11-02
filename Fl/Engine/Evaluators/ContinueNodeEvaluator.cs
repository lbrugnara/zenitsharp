// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Engine.Symbols;
using Fl.Parser.Ast;

namespace Fl.Engine.Evaluators
{
    class ContinueNodeEvaluator : INodeEvaluator<AstEvaluator, AstContinueNode, Symbol>
    {
        public Symbol Evaluate(AstEvaluator evaluator, AstContinueNode cnode)
        {
            evaluator.CurrentScope.SetContinue();
            return null;
        }
    }
}
