// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Syntax;

namespace Zenit.Ast
{
    public class AccessorNode : Node
    {
        public readonly Token Target;
        public readonly Node Parent;
        public readonly bool IsCall;

        public AccessorNode(Token target, Node parent, bool isCall = false)
        {
            this.Target = target;
            this.Parent = parent;
            this.IsCall = isCall;
        }
    }
}
