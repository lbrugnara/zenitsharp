// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Parser.Ast
{
    public class AstForNode : AstNode
    {
        public Token Keyword { get; }
        public AstNode Init { get; }
        public AstNode Condition { get; }
        public AstNode Increment { get; }
        public AstNode Body { get; }

        public AstForNode(Token keyword, AstNode init, AstNode condition, AstNode increment, AstNode body)
        {
            Keyword = keyword;
            Init = init;
            Condition = condition;
            Increment = increment;
            Body = body;
        }
    }
}
