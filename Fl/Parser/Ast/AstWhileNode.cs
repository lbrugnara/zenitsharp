// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Parser.Ast
{
    public class AstWhileNode : AstNode
    {
        public Token Keyword { get; }
        public AstNode Condition { get; }
        public AstNode Body { get; }

        public AstWhileNode(Token token, AstNode condition, AstNode body)
        {
            Keyword = token;
            Condition = condition;
            Body = body;
        }
    }
}
