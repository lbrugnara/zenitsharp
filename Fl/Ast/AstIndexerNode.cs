// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Parser;

namespace Fl.Ast
{
    public class AstIndexerNode : AstCallableNode
    {
        public AstIndexerNode(AstNode callable, AstExpressionListNode args, Token newt = null) : base(callable, args, newt)
        {
        }
    }
}
