// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Types;
using Fl.Parser.Ast;

namespace Fl.Engine.Evaluators
{
    class OperandUnaryNode
    {
        private static AccessorNodeEvaluator _AccessorEv = new AccessorNodeEvaluator();

        private AstAccessorNode GetAccessorNode(AstUnaryNode node)
        {
            var tmp = node;
            while (tmp != null && !(tmp.Left is AstAccessorNode))
            {
                tmp = tmp.Left as AstUnaryNode;
                if (tmp is AstUnaryPrefixNode || tmp is AstUnaryPostfixNode)
                    throw new AstWalkerException($"The operand of an increment/decrement operator must be a variable");
            }
            return tmp?.Left as AstAccessorNode;
        }

        public Symbol GetSymbol(AstEvaluator evaluator, AstUnaryNode node)
        {
            AstAccessorNode accessor = GetAccessorNode(node);
            if (accessor == null)
                return null;

            return _AccessorEv.GetSymbol(evaluator, accessor);
        }
    }
}
