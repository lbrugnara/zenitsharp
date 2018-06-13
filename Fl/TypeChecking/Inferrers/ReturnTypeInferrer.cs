// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols.Exceptions;
using Fl.Ast;
using Fl.Symbols.Types;
using System.Linq;

namespace Fl.TypeChecking.Inferrers
{
    class ReturnTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstReturnNode, Type>
    {
        public Type Visit(TypeInferrerVisitor visitor, AstReturnNode rnode)
        {
            if (!visitor.SymbolTable.InFunction)
                throw new ScopeOperationException("Invalid return statement in a non-function block");

//            var ret = visitor.SymbolTable.GetSymbol("@ret");

            // void
            if (rnode.ReturnTuple == null)
                return Void.Instance;

            var tupleType = rnode.ReturnTuple.Visit(visitor);

            // If tuple comes from a variable, return it as it is
            if (tupleType != null)
                return tupleType;

            // Literal tuple
            var tupleTypes = (tupleType.DataType as Tuple).Types;

            // Just one lement is like
            //  return 1;
            //  return 2;
            //  etc
            if (tupleTypes.Count == 1)
                return tupleTypes.First();

            return tupleType;
        }
    }
}
