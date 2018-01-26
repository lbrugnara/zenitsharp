// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Parser.Ast
{
    public class AstCallableNode : AstNode
    {
        public AstNode Callable { get; }
        public AstExpressionListNode Arguments { get; }
        public Token New { get; }

        public AstCallableNode(AstNode callable, AstExpressionListNode args, Token newt = null)
        {
            Callable = callable;
            Arguments = args;
            New = newt;
        }
    }
}
