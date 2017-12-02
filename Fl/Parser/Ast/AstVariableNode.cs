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
        public List<Tuple<Token, AstNode>> Variables { get; }

        public AstVariableNode(AstVariableTypeNode variableType, Token identifier, AstNode initializer)
        {
            VarType = variableType;
            Variables = new List<Tuple<Token, AstNode>>() { new Tuple<Token, AstNode>(identifier, initializer) };
        }

        public AstVariableNode(AstVariableTypeNode variableType, List<Tuple<Token, AstNode>> variables)
        {
            VarType = variableType;
            Variables = variables;
        }
    }
}
