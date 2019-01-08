// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Types;

namespace Fl.Semantics.Resolvers
{
    class ObjectPropertySymbolResolver : INodeVisitor<SymbolResolverVisitor, ObjectPropertyNode>
    {
        public void Visit(SymbolResolverVisitor visitor, ObjectPropertyNode node)
        {
            visitor.SymbolTable.Insert(
                node.Name.Value,
                SymbolHelper.GetTypeInfo(visitor.SymbolTable, visitor.Inferrer, node.Information.Type),
                Symbols.Access.Public,
                SymbolHelper.GetStorage(node.Information.Mutability)
            );

            visitor.Visit(node.Value);
        }
    }
}
