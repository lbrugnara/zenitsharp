// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;

namespace Fl.Parser.Ast
{
    public class AstVariableTypeNode : AstNode
    {
        public Token TypeToken { get; }
        public List<Token> Dimensions { get; }

        public AstVariableTypeNode(Token type, List<Token> dimensions = null)
        {
            TypeToken = type;
            Dimensions = dimensions;
        }
    }
}
