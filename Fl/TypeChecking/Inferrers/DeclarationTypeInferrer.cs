// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Lang.Types;

namespace Fl.TypeChecking.Inferrers
{
    class DeclarationTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstDeclarationNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor checker, AstDeclarationNode decls)
        {
            foreach (AstNode statement in decls.Statements)
                statement.Visit(checker);

            return null;
        }
    }
}
