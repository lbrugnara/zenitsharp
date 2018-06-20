// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;
using Fl.Semantics.Types;

namespace Fl.Semantics.Checkers
{
    class BreakTypeChecker : INodeVisitor<TypeCheckerVisitor, AstBreakNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, AstBreakNode wnode)
        {
            var nbreak = wnode.Number.Visit(checker);

            if (nbreak.Type != Int.Instance)
                throw new System.Exception($"Number of breaks must be an {Int.Instance}");

            return nbreak;
        }
    }
}
