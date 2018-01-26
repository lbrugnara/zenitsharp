// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Parser.Ast
{
    public class AstReturnNode : AstNode
    {
        public Token Keyword { get; }
        public AstTupleNode ReturnTuple { get; }

        public AstReturnNode(Token keyword, AstTupleNode expr)
        {
            Keyword = keyword;
            ReturnTuple = expr;
        }
    }
}
