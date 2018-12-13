// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Syntax;

namespace Fl.Ast
{
    public class ObjectPropertyNode : Node
    {
        public Token Name { get; set; }
        public SymbolInformation Information { get; set; }
        public Node Value { get; set; }
    }
}
