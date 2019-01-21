// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;

namespace Fl.Ast
{
    public class VariableDefinitionNode : VariableNode
    {
        public List<SymbolDefinition> Definitions { get; }

        public VariableDefinitionNode(SymbolInformation variableType, List<SymbolDefinition> definitions)
            : base(variableType)
        {
            this.Definitions = definitions;
        }
    }
}
