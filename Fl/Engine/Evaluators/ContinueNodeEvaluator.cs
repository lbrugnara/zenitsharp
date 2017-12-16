// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using Fl.Parser.Ast;

namespace Fl.Engine.Evaluators
{
    class ContinueNodeEvaluator : INodeVisitor<AstEvaluator, AstContinueNode, FlObject>
    {
        public FlObject Visit(AstEvaluator evaluator, AstContinueNode cnode)
        {
            evaluator.Symtable.SetContinue();
            return null;
        }
    }
}
