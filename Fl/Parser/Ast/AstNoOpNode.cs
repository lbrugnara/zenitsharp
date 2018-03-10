// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Parser.Ast
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
