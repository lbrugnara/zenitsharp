// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Parser.Ast;

namespace Fl.Symbols.Resolvers
{
    class TupleSymbolResolver : INodeVisitor<SymbolResolver, AstTupleNode, Symbol>
    {
        public Symbol Visit(SymbolResolver checker, AstTupleNode node)
        {
            // TODO: Fix tuple visitor
            if (node.Items.Count > 0)
            {
                foreach (var item in node.Items)
                {
                    return item.Visit(checker);
                }
            }

            return null;
        }
    }
}
