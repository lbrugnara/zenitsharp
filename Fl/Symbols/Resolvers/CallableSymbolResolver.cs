﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;

using Fl.Engine.Symbols.Types;
using Fl.Ast;
using System.Collections.Generic;
using System.Linq;

namespace Fl.Symbols.Resolvers
{
    public class CallableSymbolResolver : INodeVisitor<SymbolResolverVisitor, AstCallableNode>
    {
        public void Visit(SymbolResolverVisitor checker, AstCallableNode node)
        {
            node.Callable.Visit(checker);
            node.Arguments.Expressions.ForEach(e => e.Visit(checker));
        }
    }
}