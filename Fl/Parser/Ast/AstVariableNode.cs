// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Parser.Ast
{
    public class AstVariableNode : AstNode
    {
        public AstVariableTypeNode VarType { get; }
        public Token Identifier { get; }
        public AstNode Initializer { get; }

        public AstVariableNode(AstVariableTypeNode variableType, Token identifier, AstNode initializer)
        {
            VarType = variableType;
            Identifier = identifier;
            Initializer = initializer;
        }
    }
}
