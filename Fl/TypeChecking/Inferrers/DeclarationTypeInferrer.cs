// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Symbols.Types;

namespace Fl.TypeChecking.Inferrers
{
    class DeclarationTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstDeclarationNode, Type>
    {
        public Type Visit(TypeInferrerVisitor visitor, AstDeclarationNode decls)
        {
            foreach (AstNode statement in decls.Statements)
                statement.Visit(visitor);

            return null;
        }
    }
}
