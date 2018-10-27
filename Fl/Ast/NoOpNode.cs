// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Syntax;

namespace Fl.Ast
{
    public class NoOpNode : Node
    {
        public Token EmptyStatement { get; }

        public NoOpNode(Token emptystmt)
        {
            this.EmptyStatement = emptystmt;
        }
    }
}
