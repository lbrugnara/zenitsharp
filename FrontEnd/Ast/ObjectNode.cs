// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;

namespace Zenit.Ast
{
    public class ObjectNode : Node
    {
        public List<ObjectPropertyNode> Properties { get; set; }

        public ObjectNode(List<ObjectPropertyNode> properties)
        {
            this.Properties = properties;
        }
    }
}
