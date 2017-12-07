// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Text;

namespace Fl.Parser.Ast
{
    public abstract class AstVariableNode : AstNode
    {
        public AstVariableTypeNode VarType { get; }
        

        public AstVariableNode(AstVariableTypeNode variableType)
        {
            VarType = variableType;
        }
    }
}
