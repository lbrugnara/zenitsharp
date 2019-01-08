// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;

namespace Fl.Semantics.Resolvers
{
    class ObjectSymbolResolver : INodeVisitor<SymbolResolverVisitor, ObjectNode>
    {
        public void Visit(SymbolResolverVisitor visitor, ObjectNode node)
        {
            // Create a new ObjectSymbol and enter to the scope
            visitor.SymbolTable.EnterObjectScope(node.Uid);

            // Visit each object's property
            node.Properties.ForEach(p => visitor.Visit(p));

            // Leave the object's scope
            visitor.SymbolTable.LeaveScope();
        }
    }
}
