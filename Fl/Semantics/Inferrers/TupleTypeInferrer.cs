// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Symbols.Types;
using System.Linq;

namespace Fl.Semantics.Inferrers
{
    class TupleTypeInferrer : INodeVisitor<TypeInferrerVisitor, TupleNode, ITypeSymbol>
    {
        public ITypeSymbol Visit(TypeInferrerVisitor visitor, TupleNode node)
        {
            var inferredTypes = node.Items?.Select(i => i?.Visit(visitor));
            return new TupleSymbol(visitor.SymbolTable.CurrentScope, inferredTypes.OfType<ITypeSymbol>().Where(it => it != null).ToList());
        }
    }
}
