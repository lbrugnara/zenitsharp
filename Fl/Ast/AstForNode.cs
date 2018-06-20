// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Syntax;

namespace Fl.Ast
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
            this.Keyword = keyword;
            this.Init = init;
            this.Condition = condition;
            this.Increment = increment;
            this.Body = body;
        }
    }
}
