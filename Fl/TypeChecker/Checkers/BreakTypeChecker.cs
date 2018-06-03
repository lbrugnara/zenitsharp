// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;

using Fl.Ast;
using Fl.Lang.Types;

namespace Fl.TypeChecker.Checkers
{
    class BreakTypeChecker : INodeVisitor<TypeCheckerVisitor, AstBreakNode, Type>
    {
        public Type Visit(TypeCheckerVisitor checker, AstBreakNode wnode)
        {
            Type nbreak = wnode.Number.Visit(checker);

            if (nbreak != Lang.Types.Int.Instance)
                throw new System.Exception($"Number of breaks must be an {Lang.Types.Int.Instance}");

            return nbreak;
        }
    }
}
