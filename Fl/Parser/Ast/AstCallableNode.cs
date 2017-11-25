// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;

namespace Fl.Parser.Ast
{
    public class AstCallableNode : AstNode
    {
        public AstNode Callable { get; }
        public AstArgumentsNode Arguments { get; }
        public Token New { get; }

        public AstCallableNode(AstNode callable, AstArgumentsNode args, Token newt = null)
        {
            Callable = callable;
            Arguments = args;
            New = newt;
        }
    }
}
