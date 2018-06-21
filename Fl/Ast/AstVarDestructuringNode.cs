// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;
using Fl.Syntax;

namespace Fl.Ast
{
    public class AstVarDestructuringNode : AstVariableNode
    {
        public List<Token> Variables { get; }
        public AstNode DestructInit { get; }

        public AstVarDestructuringNode(AstTypeNode variableType, List<Token> variables, AstNode destructInit)
            : base(variableType)
        {
            this.Variables = variables;
            this.DestructInit = destructInit;
        }
    }
}
