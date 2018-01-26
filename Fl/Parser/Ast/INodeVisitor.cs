// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Parser.Ast
{
    public interface INodeVisitor<W, N, R> 
        where N : AstNode 
        where R : class 
        where W : IAstWalker<R>
    {
        R Visit(W walker, N node);
    }
}
