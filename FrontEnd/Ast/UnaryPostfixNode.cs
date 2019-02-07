// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Syntax;

namespace Zenit.Ast
{
    public class UnaryPostfixNode : UnaryNode
    {
        public UnaryPostfixNode(Token opToken, Node left)
            : base(opToken, left)
        {

        }
    }
}
