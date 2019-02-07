﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Symbols.Types;

namespace Zenit.Semantics.Inferrers
{
    class BreakTypeInferrer : INodeVisitor<TypeInferrerVisitor, BreakNode, ITypeSymbol>
    {
        public ITypeSymbol Visit(TypeInferrerVisitor visitor, BreakNode wnode)
        {
            return wnode.Number?.Visit(visitor);
        }
    }
}