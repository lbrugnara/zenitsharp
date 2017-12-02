// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;

namespace Fl.Parser.Ast
{
    public class AstTupleNode : AstNode
    {
        public List<AstNode> Items { get; }

        public AstTupleNode(List<AstNode> init)
        {
            Items = init;
        }

        public AstTupleNode(AstExpressionListNode exprlist)
        {
            Items = exprlist.Expressions;
        }
    }
}
