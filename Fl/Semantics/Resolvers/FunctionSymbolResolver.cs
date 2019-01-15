// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Linq;
using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Symbols.Values;
using Fl.Semantics.Types;

namespace Fl.Semantics.Resolvers
{
    class FunctionSymbolResolver : INodeVisitor<SymbolResolverVisitor, FunctionNode, IValueSymbol>
    {
        public IValueSymbol Visit(SymbolResolverVisitor visitor, FunctionNode funcdecl)
        {
            // Change the current scope to be the function's scope
            var functionSymbol = visitor.SymbolTable.EnterFunctionScope(funcdecl.Name);

            // Process the parameters
            funcdecl.Parameters.ForEach(p => this.CreateParameterSymbol(visitor, functionSymbol, p));

            // Visit the function's body
            var exprs = funcdecl.Body.Select(s => s.Visit(visitor)).ToList();

            if (funcdecl.IsLambda)
            {
                var lastExpr = exprs.LastOrDefault();

                if (lastExpr == null)
                {
                    //   lastExpr = visitor.Inferrer.NewAnonymousTypeFor(functionSymbol.Return);
                    visitor.Inferrer.NewAnonymousTypeFor(functionSymbol.Return);
                }
                else
                {
                    functionSymbol.Return.ChangeType(lastExpr is ITypeSymbol lets ? lets : (lastExpr as IBoundSymbol).TypeSymbol);
                    // var generalType = visitor.Inferrer.FindMostGeneralType(functionSymbol.Return.TypeSymbol, lastExpr);
                    // visitor.Inferrer.Unify(visitor.SymbolTable, generalType, functionSymbol.Return);
                }                
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
