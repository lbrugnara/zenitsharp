// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Zenit.Ast
{
    public interface INodeVisitor<W, N>
        where N : Node
        where W : IAstWalker
    {
        void Visit(W walker, N node);
    }

    public interface INodeVisitor<W, N, R> 
        where N : Node 
        where R : class 
        where W : IAstWalker<R>
    {
        R Visit(W walker, N node);
    }
}
