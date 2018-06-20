// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Syntax;

namespace Fl.Ast
{
    public class AstNoOpNode : AstNode
    {
        public Token EmptyStatement { get; }

        public AstNoOpNode(Token emptystmt)
        {
            this.EmptyStatement = emptystmt;
        }
    }
}
