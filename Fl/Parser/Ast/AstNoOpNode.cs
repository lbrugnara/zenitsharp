// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;

namespace Fl.Parser.Ast
{
    public class AstNoOpNode : AstNode
    {
        public Token EmptyStatement { get; }

        public AstNoOpNode(Token semicolon)
        {
            EmptyStatement = semicolon;
        }
    }
}
