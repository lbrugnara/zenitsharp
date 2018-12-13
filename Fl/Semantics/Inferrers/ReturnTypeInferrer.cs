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

            // The current scope is the function's scope. We get a reference to the
            // return type and we update it if needed
            var functionScope = visitor.SymbolTable.CurrentFunctionScope;

            // If there's an empty return statement, leave here
            if (rnode.Expression == null)
            {
                visitor.Inferrer.Unify(Void.Instance, functionScope.ReturnSymbol.TypeInfo);
                // again, we assume the first return's expression type is the function's return type
                functionScope.UpdateReturnType(functionScope.ReturnSymbol.TypeInfo);

                return new InferredType(functionScope.ReturnSymbol.TypeInfo);
            }

            // Infer the return's expression type
            var returnInferredType = rnode.Expression.Visit(visitor);
            
            TypeInfo typeInfo = returnInferredType.TypeInfo;

            // The return statement expects a tuple and if that tuple contains
            // just one element, we use it as the return's type
            if ((typeInfo.Type is Tuple t) && t.Types.Count == 1)
                typeInfo.Type = t.Types.First();

            visitor.Inferrer.Unify(typeInfo, functionScope.ReturnSymbol.TypeInfo);

            // again, we assume the first return's expression type is the function's return type
            functionScope.UpdateReturnType(typeInfo);

            // If the @ret type is an assumption, register the symbol under that assumption too
            /*if (visitor.Inferrer.IsTypeAssumption(functionScope.ReturnSymbol.TypeInfo))
                visitor.Inferrer.AddTypeDependency(functionScope.ReturnSymbol.TypeInfo, functionScope.ReturnSymbol);*/

            // The type we infer from the return expression must honor the @ret symbol's type
            // as we are "leaving" the function, and the return type must be the first we processed
            return new InferredType(functionScope.ReturnSymbol.TypeInfo, returnInferredType.Symbol);
        }
    }
}
