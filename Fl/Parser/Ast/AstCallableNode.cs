// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;

namespace Fl.Parser.Ast
{
    public class AstCallableNode : AstNode
    {
        public AstNode Callable { get; }
        public AstExpressionList Arguments { get; }
        public Token New { get; }

        public AstCallableNode(AstNode callable, AstExpressionList args, Token newt = null)
        {
            Callable = callable;
            Arguments = args;
            New = newt;
        }
    }

    public class AstIndexerNode : AstCallableNode
    {
        public AstIndexerNode(AstNode callable, AstExpressionList args, Token newt = null) : base(callable, args, newt)
        {
        }
    }
}
