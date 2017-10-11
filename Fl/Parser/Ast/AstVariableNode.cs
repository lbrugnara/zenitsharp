// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;
using System.Text;

namespace Fl.Parser.Ast
{
    public class AstVariableNode : AstNode
    {
        public Token Identifier { get; }
        public AstNode Initializer { get; }

        public AstVariableNode(Token identifier, AstNode initializer)
        {
            Identifier = identifier;
            Initializer = initializer;
        }
    }
}
