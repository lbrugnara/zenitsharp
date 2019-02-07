// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Syntax;

namespace Zenit.Ast
{
    public class UnaryPrefixNode : UnaryNode
    {
        public UnaryPrefixNode(Token opToken, Node left)
            : base (opToken, left)
        {

        }
    }
}
