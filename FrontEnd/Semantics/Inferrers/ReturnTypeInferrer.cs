﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Symbols.Types;
using Zenit.Semantics.Symbols.Types.Specials;

namespace Zenit.Semantics.Inferrers
{
    class ReturnTypeInferrer : INodeVisitor<TypeInferrerVisitor, ReturnNode, IType>
    {
        public IType Visit(TypeInferrerVisitor visitor, ReturnNode rnode)
        {
            // The current scope is the function's scope. We get a reference to the
            // return type and we update it if needed
            var functionScope = visitor.SymbolTable.GetCurrentFunction();

            // If there's an empty return statement, leave here
            if (rnode.Expression == null)
            {
                visitor.Inferrer.Unify(visitor.SymbolTable, new Void(), functionScope.Return);

                return functionScope.Return.TypeSymbol;
            }

            // Infer the return's expression type
            var returnIValueSymbol = rnode.Expression.Visit(visitor);
            
            IType typeInfo = returnIValueSymbol;

            // The return statement expects a tuple and if that tuple contains
            // just one element, we use it as the return's type
            //if ((typeInfo is TupleSymbol t) && t.Types.Count == 1)
            //    typeInfo.ChangeType(t.Types.First());

            var generalType = visitor.Inferrer.FindMostGeneralType(typeInfo, functionScope.Return.TypeSymbol);

            visitor.Inferrer.Unify(visitor.SymbolTable, generalType, functionScope.Return);

            // If the @ret type is an assumption, register the symbol under that assumption too
            /*if (visitor.Inferrer.IsTypeAssumption(functionScope.Return.ITypeSymbol))
                visitor.Inferrer.AddTypeDependency(functionScope.Return.ITypeSymbol, functionScope.Return);*/

            // The type we infer from the return expression must honor the @ret symbol's type
            // as we are "leaving" the function, and the return type must be the first we processed
            return functionScope.Return.TypeSymbol;
        }
    }
}
