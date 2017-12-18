// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

namespace Fl.Parser.Ast
{
    public abstract class AstNode
    {
        public T Exec<T>(IAstWalker<T> walker) where T : class
        {
            return walker.Visit(this);
        }
    }
}
