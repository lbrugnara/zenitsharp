// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;

namespace Fl.Parser.Ast
{
    public class AstArgumentsNode : AstNode
    {
        public List<AstNode> Expressions { get; }

        public AstArgumentsNode(List<AstNode> args)
        {
            Expressions = args;
        }
    }
}
