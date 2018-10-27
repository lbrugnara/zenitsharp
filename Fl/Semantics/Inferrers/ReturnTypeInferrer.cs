// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Exceptions;
using Fl.Ast;
using Fl.Semantics.Types;
using System.Linq;
using Fl.Semantics.Symbols;

namespace Fl.Semantics.Inferrers
{
    class ReturnTypeInferrer : INodeVisitor<TypeInferrerVisitor, ReturnNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, ReturnNode rnode)
        {
            if (!visitor.SymbolTable.InFunction)
                throw new ScopeOperationException("Invalid return statement in a non-function block");

            var returnSymbol = visitor.SymbolTable.GetSymbol("@ret");

            Type type = null;
            Symbol symbol = null;

            var returnInferredType = rnode.Expression?.Visit(visitor);

            // void
            if (returnInferredType == null)
                type = Void.Instance;
            else
                type = returnInferredType?.Type;

            symbol = returnInferredType?.Symbol;

            // Just one lement is like
            //  return 1;
            //  return 2;
            //  etc
            if ((type is Tuple t) && t.Types.Count == 1)
                type = t.Types.First();

            visitor.Inferrer.MakeConclusion(type, returnSymbol.Type);

            return new InferredType(type, symbol);
        }
    }
}
