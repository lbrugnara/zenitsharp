// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;

namespace Fl.Semantics.Resolvers
{
    class ObjectSymbolResolver : INodeVisitor<SymbolResolverVisitor, ObjectNode>
    {
        public void Visit(SymbolResolverVisitor visitor, ObjectNode node)
        {
            visitor.SymbolTable.EnterObjectScope($"object-{node.GetHashCode()}");

            node.Properties.ForEach(p => visitor.Visit(p));

            visitor.SymbolTable.LeaveScope();
        }
    }
}
