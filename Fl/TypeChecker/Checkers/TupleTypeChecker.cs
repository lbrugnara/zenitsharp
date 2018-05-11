// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Ast;

namespace Fl.TypeChecker.Checkers
{
    class TupleTypeChecker : INodeVisitor<TypeChecker, AstTupleNode, Symbol>
    {
        public Symbol Visit(TypeChecker checker, AstTupleNode node)
        {
            // TODO: Fix tuple visitor
            if (node.Items.Count > 0)
            {
                foreach (var item in node.Items)
                {
                    return item.Visit(checker);
                }
            }

            return null;
        }
    }
}
