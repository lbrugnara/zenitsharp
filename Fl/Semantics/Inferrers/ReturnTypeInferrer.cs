// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Exceptions;
using Fl.Ast;
using Fl.Semantics.Types;
using System.Linq;

namespace Fl.Semantics.Inferrers
{
    class ReturnTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstReturnNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, AstReturnNode rnode)
        {
            if (!visitor.SymbolTable.InFunction)
                throw new ScopeOperationException("Invalid return statement in a non-function block");

//            var ret = visitor.SymbolTable.GetSymbol("@ret");

            // void
            if (rnode.ReturnTuple == null)
                return new InferredType(Void.Instance);

            var tupleType = rnode.ReturnTuple.Visit(visitor);

            if (tupleType.Symbol != null)
                return tupleType;

            // Literal tuple
            var tupleTypes = (tupleType.Type as Tuple).Types;

            // Just one lement is like
            //  return 1;
            //  return 2;
            //  etc
            if (tupleTypes.Count == 1)
                return new InferredType(tupleTypes.First());

            return tupleType;
        }
    }
}
