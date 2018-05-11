// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;
using System.Collections.Generic;
using Fl.Parser;

namespace Fl.Ast
{
    public class AstVarDefinitionNode : AstVariableNode
    {
        public List<Tuple<Token, AstNode>> VarDefinitions { get; }

        public AstVarDefinitionNode(AstVariableTypeNode variableType, List<Tuple<Token, AstNode>> vardefs)
            : base(variableType)
        {
            this.VarDefinitions = vardefs;
        }
    }
}
