// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Types;

namespace Fl.Semantics.Inferrers
{
    class DeclarationTypeInferrer : INodeVisitor<TypeInferrerVisitor, DeclarationNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, DeclarationNode decls)
        {
            decls.Statements.ForEach(statement => statement.Visit(visitor));

            return null;
        }
    }
}
