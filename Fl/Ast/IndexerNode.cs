// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Syntax;

namespace Fl.Ast
{
    public class IndexerNode : CallableNode
    {
        public IndexerNode(Node callable, ExpressionListNode args, Token newt = null) : base(callable, args, newt)
        {
        }
    }
}
