// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Syntax;

namespace Fl.Ast
{
    public class UnaryPrefixNode : UnaryNode
    {
        public UnaryPrefixNode(Token opToken, Node left)
            : base (opToken, left)
        {

        }
    }
}
