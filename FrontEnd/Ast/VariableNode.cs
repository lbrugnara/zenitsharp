// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Zenit.Ast
{
    public abstract class VariableNode : Node
    {
        public SymbolInformation Information { get; }

        public VariableNode(SymbolInformation info)
        {
            this.Information = info;
        }
    }
}
