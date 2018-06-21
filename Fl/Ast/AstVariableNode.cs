// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Ast
{
    public abstract class AstVariableNode : AstNode
    {
        public AstTypeNode VarType { get; }
        

        public AstVariableNode(AstTypeNode variableType)
        {
            this.VarType = variableType;
        }
    }
}
