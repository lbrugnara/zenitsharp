// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols.Types;

namespace Fl.Semantics.Inferrers
{
    class BreakTypeInferrer : INodeVisitor<TypeInferrerVisitor, BreakNode, ITypeSymbol>
    {
        public ITypeSymbol Visit(TypeInferrerVisitor visitor, BreakNode wnode)
        {
            return wnode.Number?.Visit(visitor);
        }
    }
}
