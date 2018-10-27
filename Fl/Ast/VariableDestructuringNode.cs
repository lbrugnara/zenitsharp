// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;
using Fl.Syntax;

namespace Fl.Ast
{
    public class VariableDestructuringNode : VariableNode
    {
        public List<Token> Left { get; }
        public Node Right { get; }

        public VariableDestructuringNode(SymbolInformation variableType, List<Token> variables, Node destructInit)
            : base(variableType)
        {
            this.Left = variables;
            this.Right = destructInit;
        }
    }
}
