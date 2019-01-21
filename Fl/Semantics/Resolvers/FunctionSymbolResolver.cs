﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Linq;
using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Symbols;
using Fl.Semantics.Symbols.Types.Specials;
using Fl.Semantics.Types;

namespace Fl.Semantics.Resolvers
{
    class FunctionSymbolResolver : INodeVisitor<SymbolResolverVisitor, FunctionNode, ISymbol>
    {
        public ISymbol Visit(SymbolResolverVisitor visitor, FunctionNode funcdecl)
        {
            // Change the current scope to be the function's scope
            var functionSymbol = visitor.SymbolTable.EnterFunctionScope(funcdecl.Name);

            // Process the parameters
            funcdecl.Parameters.ForEach(p => this.CreateParameterSymbol(visitor, functionSymbol, p));

            // Visit the function's body
            var exprs = funcdecl.Body.Select(s => s.Visit(visitor)).ToList();

            if (funcdecl.IsLambda)
            {
                // If it is a lambda expression, we will check the expression's type
                // to determine the function's type
                var lastExpr = exprs.Last();

                // We do allow lambdas that do not return a value
                if (lastExpr != null)
                    functionSymbol.Return.ChangeType(lastExpr.IsOfType<VoidSymbol>() ? new VoidSymbol() : lastExpr.GetTypeSymbol());
            }
            else
            {
                if (functionSymbol.Return.TypeSymbol.BuiltinType == BuiltinType.None)
                    functionSymbol.Return.ChangeType(new VoidSymbol());
            }

            // Restore previous scope
            visitor.SymbolTable.LeaveScope();

            // Trigger a lookup to update unresolved symbols
            visitor.SymbolTable.UpdateSymbolReferences();

            return functionSymbol;
        }

        private void CreateParameterSymbol(SymbolResolverVisitor visitor, FunctionSymbol functionSymbol, ParameterNode parameter)
        {
            // If the parameter's type is present use it, if not use an anonymous type
            var paramTypeSymbol = parameter.SymbolInfo.Type == null
                        ? visitor.Inferrer.NewAnonymousTypeFor()
                        : SymbolHelper.GetTypeSymbol(visitor.SymbolTable, visitor.Inferrer, parameter.SymbolInfo.Type);

            // Check if the parameter has storage modifiers
            var storage = SymbolHelper.GetStorage(parameter.SymbolInfo.Mutability);

            // Create the parameter symbol in the function's scope
            functionSymbol.CreateParameter(parameter.Name.Value, paramTypeSymbol, storage);
        }
    }
}
