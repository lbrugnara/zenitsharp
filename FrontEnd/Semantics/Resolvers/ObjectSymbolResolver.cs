// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Symbols;

namespace Zenit.Semantics.Resolvers
{
    class ObjectSymbolResolver : INodeVisitor<SymbolResolverVisitor, ObjectNode, ISymbol>
    {
        public ISymbol Visit(SymbolResolverVisitor visitor, ObjectNode node)
        {
            // Create a new ObjectSymbol and enter to the scope
            var objectSymbol = visitor.SymbolTable.EnterObjectScope(node.Uid);

            // Visit each object's property
            node.Properties.ForEach(p => visitor.Visit(p));

            // Leave the object's scope
            visitor.SymbolTable.LeaveScope();

            return objectSymbol;
        }
    }
}
