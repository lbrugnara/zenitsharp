// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Symbols.Types;

namespace Zenit.Semantics.Inferrers
{
    class ContinueTypeInferrer : INodeVisitor<TypeInferrerVisitor, ContinueNode, IType>
    {
        public IType Visit(TypeInferrerVisitor visitor, ContinueNode cnode)
        {
            // continue is an empty statement
            return null;
        }
    }
}
