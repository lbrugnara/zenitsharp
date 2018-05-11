// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Ast
{
    public abstract class AstVariableNode : AstNode
    {
        public AstVariableTypeNode VarType { get; }
        

        public AstVariableNode(AstVariableTypeNode variableType)
        {
            this.VarType = variableType;
        }
    }
}
