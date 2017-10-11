// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;

namespace Fl.Parser.Ast
{
    public class AstCallableNode : AstNode
    {
        public AstNode Callable { get; }
        public AstArgumentsNode Arguments { get; }

        public AstCallableNode(AstNode callable, AstArgumentsNode args)
        {
            Callable = callable;
            Arguments = args;
        }
    }
}
