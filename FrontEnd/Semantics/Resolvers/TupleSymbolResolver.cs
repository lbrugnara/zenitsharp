// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Symbols;
using Zenit.Semantics.Symbols.Variables;

namespace Zenit.Semantics.Resolvers
{
    class TupleSymbolResolver : INodeVisitor<SymbolResolverVisitor, TupleNode, ISymbol>
    {
        public ISymbol Visit(SymbolResolverVisitor visitor, TupleNode node)
        {
            // Create a new Tuple and enter to the scope
            var tuple = visitor.SymbolTable.EnterTupleScope(node.Uid);

            node.Items?.ForEach(item => {
                var el = item.Expression.Visit(visitor);

                // We are using the default name, but we can extend this to support named elements within tuples
                var name = item.Name ?? $"${tuple.Count}";

                visitor.SymbolTable.AddNewVariableSymbol(name, el.GetTypeSymbol(), Access.Public, Storage.Immutable);
            });

            // Leave the tuple's scope
            visitor.SymbolTable.LeaveScope();

            return tuple;
        }
    }
}
