// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Exceptions;
using Fl.Ast;
using Fl.Lang.Types;
using System.Linq;

namespace Fl.TypeChecking.Inferrers
{
    class ReturnTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstReturnNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, AstReturnNode rnode)
        {
            if (!visitor.SymbolTable.CurrentBlock.IsFunction)
                throw new ScopeOperationException("Invalid return statement in a non-function block");

            var ret = visitor.SymbolTable.GetSymbol("@ret");

            // void
            if (rnode.ReturnTuple == null)
            {
                ret.Type = Null.Instance;
                return null;
            }

            var tupleInferredType = rnode.ReturnTuple.Visit(visitor);

            // Set return type to the same type of the tuple
            ret.Type = tupleInferredType.Type;

            // If tuple comes from a variable, return it as it is
            if (tupleInferredType.Symbol != null)
                return tupleInferredType;

            // Literal tuple
            var tupleTypes = (tupleInferredType.Type as Tuple).Types;

            // Just one lement is like
            //  return 1;
            //  return 2;
            //  etc
            if (tupleTypes.Count == 1)
            {
                tupleInferredType = new InferredType(tupleTypes.First());
                // Set return type to the same type of the first type in the tuple
                ret.Type = tupleInferredType.Type;
            }

            return tupleInferredType;
        }
    }
}
