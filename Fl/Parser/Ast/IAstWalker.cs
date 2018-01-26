// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Parser.Ast
{
    public interface IAstWalker<T> where T : class
    {
        T Visit(AstNode node);
    }
}
