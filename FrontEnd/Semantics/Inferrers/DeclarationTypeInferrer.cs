// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Symbols.Types;

namespace Zenit.Semantics.Inferrers
{
    class DeclarationTypeInferrer : INodeVisitor<TypeInferrerVisitor, DeclarationNode, ITypeSymbol>
    {
        public ITypeSymbol Visit(TypeInferrerVisitor visitor, DeclarationNode decls)
        {
            decls.Statements.ForEach(statement => statement.Visit(visitor));

            return null;
        }
    }
}
