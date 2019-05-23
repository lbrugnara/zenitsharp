// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Symbols;
using Zenit.Semantics.Symbols.Types;
using System.Linq;
using Zenit.Semantics.Symbols.Types.References;

namespace Zenit.Semantics.Inferrers
{
    class TupleTypeInferrer : INodeVisitor<TypeInferrerVisitor, TupleNode, IType>
    {
        public IType Visit(TypeInferrerVisitor visitor, TupleNode node)
        {
            var inferredTypes = node.Items?.Select(i => i?.Expression?.Visit(visitor));

            return new Tuple(node.Uid, visitor.SymbolTable.CurrentScope, inferredTypes.OfType<IType>().Where(it => it != null).Cast<ISymbol>().ToList());
        }
    }
}
