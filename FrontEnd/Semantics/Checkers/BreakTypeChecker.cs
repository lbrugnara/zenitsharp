// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Zenit.Ast;
using Zenit.Semantics.Types;

namespace Zenit.Semantics.Checkers
{
    class BreakTypeChecker : INodeVisitor<TypeCheckerVisitor, BreakNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, BreakNode wnode)
        {
            var nbreak = wnode.Number.Visit(checker);

            if (nbreak.TypeSymbol.BuiltinType != BuiltinType.Int)
                throw new System.Exception($"Number of breaks must be an {BuiltinType.Int.GetName()}");

            return nbreak;
        }
    }
}
