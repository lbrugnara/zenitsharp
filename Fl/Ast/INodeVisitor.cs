// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Ast
{
    public interface INodeVisitor<W, N>
        where N : AstNode
        where W : IAstWalker
    {
        void Visit(W walker, N node);
    }

    public interface INodeVisitor<W, N, R> 
        where N : AstNode 
        where R : class 
        where W : IAstWalker<R>
    {
        R Visit(W walker, N node);
    }
}
