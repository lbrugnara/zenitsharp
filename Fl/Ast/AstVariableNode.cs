// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Ast
{
    public abstract class AstVariableNode : AstNode
    {
        public SymbolInformation SymbolInfo { get; }
        

        public AstVariableNode(SymbolInformation variableType)
        {
            this.SymbolInfo = variableType;
        }
    }
}
