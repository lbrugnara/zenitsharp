// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;

using Fl.Engine.Symbols.Types;
using Fl.Parser.Ast;
using System.Collections.Generic;
using System.Linq;

namespace Fl.Symbols.Resolvers
{
    public class CallableSymbolResolver : INodeVisitor<SymbolResolver, AstCallableNode, Symbol>
    {
        public Symbol Visit(SymbolResolver checker, AstCallableNode node)
        {
            Symbol target = node.Callable.Visit(checker);
            node.Arguments.Expressions.ForEach(e => e.Visit(checker));
            return target;
        }
    }
}
