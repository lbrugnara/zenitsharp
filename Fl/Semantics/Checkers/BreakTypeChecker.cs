// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;
using Fl.Semantics.Types;

namespace Fl.Semantics.Checkers
{
    class BreakTypeChecker : INodeVisitor<TypeCheckerVisitor, BreakNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, BreakNode wnode)
        {
            var nbreak = wnode.Number.Visit(checker);

            if (nbreak.TypeInfo.Type != Int.Instance)
                throw new System.Exception($"Number of breaks must be an {Int.Instance}");

            return nbreak;
        }
    }
}
