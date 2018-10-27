// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Ast
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
