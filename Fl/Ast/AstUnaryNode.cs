// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Parser;

namespace Fl.Ast
{
    public class AstUnaryNode : AstNode
    {
        public Token Operator { get; }
        public AstNode Left { get; }

        public AstUnaryNode(Token opToken, AstNode left)
        {
            this.Operator = opToken;
            this.Left = left;
        }
    }
}
