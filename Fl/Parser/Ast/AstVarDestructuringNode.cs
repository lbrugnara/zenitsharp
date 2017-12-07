// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;

namespace Fl.Parser.Ast
{
    public class AstVarDestructuringNode : AstVariableNode
    {
        public List<Token> Variables { get; }
        public AstNode DestructInit { get; }

        public AstVarDestructuringNode(AstVariableTypeNode variableType, List<Token> variables, AstNode destructInit)
            : base(variableType)
        {
            Variables = variables;
            DestructInit = destructInit;
        }
    }
}
