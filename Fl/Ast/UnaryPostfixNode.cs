// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Syntax;

namespace Fl.Ast
{
    public class UnaryPostfixNode : UnaryNode
    {
        public UnaryPostfixNode(Token opToken, Node left)
            : base(opToken, left)
        {

        }
    }
}
