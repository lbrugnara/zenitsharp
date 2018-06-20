// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Syntax;

namespace Fl.Ast
{
    public class AstReturnNode : AstNode
    {
        public Token Keyword { get; }
        public AstTupleNode ReturnTuple { get; }

        public AstReturnNode(Token keyword, AstTupleNode expr)
        {
            this.Keyword = keyword;
            this.ReturnTuple = expr;
        }
    }
}
