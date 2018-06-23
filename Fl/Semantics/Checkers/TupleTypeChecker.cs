// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Types;
using System.Linq;

namespace Fl.Semantics.Checkers
{
    class TupleTypeChecker : INodeVisitor<TypeCheckerVisitor, AstTupleNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, AstTupleNode node)
        {
            var types = node.Items?.Select(i => i?.Visit(checker)?.Type);
            return new CheckedType(new Tuple(types.ToArray()));
        }
    }
}
