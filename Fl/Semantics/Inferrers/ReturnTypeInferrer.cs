// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Exceptions;
using Fl.Ast;
using Fl.Semantics.Types;
using System.Linq;
using Fl.Semantics.Symbols;

namespace Fl.Semantics.Inferrers
{
    class ReturnTypeInferrer : INodeVisitor<TypeInferrerVisitor, ReturnNode, ITypeSymbol>
    {
        public ITypeSymbol Visit(TypeInferrerVisitor visitor, ReturnNode rnode)
        {
            if (!visitor.SymbolTable.InFunction)
                throw new ScopeOperationException("Invalid return statement in a non-function block");

            // The current scope is the function's scope. We get a reference to the
            // return type and we update it if needed
            var functionScope = visitor.SymbolTable.CurrentScope as FunctionSymbol;

            // If there's an empty return statement, leave here
            if (rnode.Expression == null)
            {
                //visitor.Inferrer.ExpectsToUnifyWith(functionScope.ReturnSymbol.TypeSymbol, BuiltinType.Void);
                // again, we assume the first return's expression type is the function's return type
                functionScope.UpdateReturnType(functionScope.Return.TypeSymbol);

                return functionScope.Return.TypeSymbol;
            }

            // Infer the return's expression type
            var returnIValueSymbol = rnode.Expression.Visit(visitor);
            
            ITypeSymbol typeInfo = returnIValueSymbol;

            // The return statement expects a tuple and if that tuple contains
            // just one element, we use it as the return's type
            //if ((typeInfo is TupleSymbol t) && t.Types.Count == 1)
            //    typeInfo.ChangeType(t.Types.First());

            visitor.Inferrer.FindMostGeneralType(typeInfo, functionScope.Return.TypeSymbol);

            // again, we assume the first return's expression type is the function's return type
            functionScope.UpdateReturnType(typeInfo);

            // If the @ret type is an assumption, register the symbol under that assumption too
            /*if (visitor.Inferrer.IsTypeAssumption(functionScope.Return.ITypeSymbol))
                visitor.Inferrer.AddTypeDependency(functionScope.Return.ITypeSymbol, functionScope.Return);*/

            // The type we infer from the return expression must honor the @ret symbol's type
            // as we are "leaving" the function, and the return type must be the first we processed
            return functionScope.Return.TypeSymbol;
        }
    }
}
