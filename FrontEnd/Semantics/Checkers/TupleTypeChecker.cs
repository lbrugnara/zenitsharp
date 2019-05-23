// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Symbols;
using Zenit.Semantics.Symbols.Types;
using System.Linq;
using Zenit.Semantics.Symbols.Types.References;

namespace Zenit.Semantics.Checkers
{
    class TupleTypeChecker : INodeVisitor<TypeCheckerVisitor, TupleNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, TupleNode node)
        {
            var elements = node.Items?.Select(i => i?.Expression?.Visit(checker)?.Symbol).Cast<ISymbol>();
            return new CheckedType(new Tuple(node.Uid, checker.SymbolTable.CurrentScope, elements.ToList()));
        }
    }
}
