// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Symbols.Types;

namespace Zenit.Semantics.Inferrers
{
    class DeclarationTypeInferrer : INodeVisitor<TypeInferrerVisitor, DeclarationNode, IType>
    {
        public IType Visit(TypeInferrerVisitor visitor, DeclarationNode decls)
        {
            decls.Declarations.ForEach(statement => statement.Visit(visitor));

            return null;
        }
    }
}
