// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Syntax;

namespace Fl.Ast
{
    public class ReturnNode : Node
    {
        public Token Keyword { get; }
        public TupleNode Expression { get; }

        public ReturnNode(Token keyword, TupleNode expr)
        {
            this.Keyword = keyword;
            this.Expression = expr;
        }
    }
}
