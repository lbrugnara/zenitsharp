// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Ast
{
    public interface IAstWalker
    {
        void Visit(Node node);
    }

    public interface IAstWalker<T> where T : class
    {
        T Visit(Node node);
    }
}
