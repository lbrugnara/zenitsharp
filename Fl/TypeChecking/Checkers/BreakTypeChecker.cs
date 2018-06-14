// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;
using Fl.Symbols.Types;

namespace Fl.TypeChecking.Checkers
{
    class BreakTypeChecker : INodeVisitor<TypeCheckerVisitor, AstBreakNode, SType>
    {
        public SType Visit(TypeCheckerVisitor checker, AstBreakNode wnode)
        {
            SType nbreak = wnode.Number.Visit(checker);

            if (nbreak != Symbols.Types.Int.Instance)
                throw new System.Exception($"Number of breaks must be an {Symbols.Types.Int.Instance}");

            return nbreak;
        }
    }
}
