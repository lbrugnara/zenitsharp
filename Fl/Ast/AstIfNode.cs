// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Parser;

namespace Fl.Ast
{
    public class AstIfNode : AstNode
    {
        public Token Keyword { get; }
        public AstNode Condition { get; }
        public AstNode Then { get; }
        public AstNode Else { get; }

        public AstIfNode(Token keyword, AstNode condition, AstNode thenbranch, AstNode elsebranch)
        {
            this.Keyword = keyword;
            this.Condition = condition;
            this.Then = thenbranch;
            this.Else = elsebranch;
        }
    }
}
