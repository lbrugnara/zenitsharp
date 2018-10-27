// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Syntax;
using System.Collections.Generic;

namespace Fl.Ast
{
    class ClassNode : Node
    {
        public Token Name { get; }
        public List<ClassPropertyNode> Properties { get; }
        public List<ClassConstantNode> Constants { get; }
        public List<ClassMethodNode> Methods { get; }

        public ClassNode(Token className, List<ClassPropertyNode> properties, List<ClassConstantNode> constants, List<ClassMethodNode> methods)
        {
            this.Name = className;
            this.Properties = properties;
            this.Constants = constants;
            this.Methods = methods;
        }
    }
}
