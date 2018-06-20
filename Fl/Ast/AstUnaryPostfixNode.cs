// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Syntax;

namespace Fl.Ast
{
    public class AstUnaryPostfixNode : AstUnaryNode
    {
        public AstUnaryPostfixNode(Token opToken, AstNode left)
            : base(opToken, left)
        {

        }
    }
}
