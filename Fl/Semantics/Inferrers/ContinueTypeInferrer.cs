// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols.Types;

namespace Fl.Semantics.Inferrers
{
    class ContinueTypeInferrer : INodeVisitor<TypeInferrerVisitor, ContinueNode, ITypeSymbol>
    {
        public ITypeSymbol Visit(TypeInferrerVisitor visitor, ContinueNode cnode)
        {
            // continue is an empty statement
            return null;
        }
    }
}
