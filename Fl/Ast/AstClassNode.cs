// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Syntax;
using System.Collections.Generic;

namespace Fl.Ast
{
    class AstClassNode : AstNode
    {
        public Token Name { get; }
        public List<AstClassPropertyNode> Properties { get; }
        public List<AstClassConstantNode> Constants { get; }
        public List<AstClassMethodNode> Methods { get; }

        public AstClassNode(Token className, List<AstClassPropertyNode> properties, List<AstClassConstantNode> constants, List<AstClassMethodNode> methods)
        {
            this.Name = className;
            this.Properties = properties;
            this.Constants = constants;
            this.Methods = methods;
        }
    }
}
