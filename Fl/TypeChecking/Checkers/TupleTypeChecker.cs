// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Symbols.Types;
using System.Linq;

namespace Fl.TypeChecking.Checkers
{
    class TupleTypeChecker : INodeVisitor<TypeCheckerVisitor, AstTupleNode, Type>
    {
        public Type Visit(TypeCheckerVisitor checker, AstTupleNode node)
        {
            var types = node.Items?.Select(i => i.Visit(checker));
            // TODO: Handle tuple type
            return new Tuple(types.ToArray());
        }
    }
}
