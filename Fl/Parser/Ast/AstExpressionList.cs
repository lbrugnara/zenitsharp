// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;

namespace Fl.Parser.Ast
{
    public class AstExpressionList : AstNode
    {
        public List<AstNode> Expressions { get; }

        public AstExpressionList(List<AstNode> args)
        {
            Expressions = args;
        }

        public int Count => Expressions?.Count ?? 0;
    }
}
