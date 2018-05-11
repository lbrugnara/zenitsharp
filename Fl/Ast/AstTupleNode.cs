// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;

namespace Fl.Ast
{
    public class AstTupleNode : AstNode
    {
        public List<AstNode> Items { get; }

        public AstTupleNode(List<AstNode> init)
        {
            this.Items = init;
        }

        public AstTupleNode(AstExpressionListNode exprlist)
        {
            this.Items = exprlist.Expressions;
        }
    }
}
