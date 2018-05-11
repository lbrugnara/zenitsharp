// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;

namespace Fl.Ast
{
    public class AstExpressionListNode : AstNode
    {
        public List<AstNode> Expressions { get; }

        public AstExpressionListNode(List<AstNode> args)
        {
            this.Expressions = args;
        }

        public int Count => this.Expressions?.Count ?? 0;
    }
}
