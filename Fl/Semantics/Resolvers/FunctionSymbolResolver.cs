// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Resolvers
{
    class FunctionSymbolResolver : INodeVisitor<SymbolResolverVisitor, FunctionNode>
    {
        public void Visit(SymbolResolverVisitor visitor, FunctionNode funcdecl)
        {
            // Change the current scope to be the function's scope
            var functionSymbol = visitor.SymbolTable.EnterFunctionScope(funcdecl.Name);

            // Process the parameters
            funcdecl.Parameters.ForEach(p => this.CreateParameterSymbol(visitor, functionSymbol, p));

            // Visit the function's body
            funcdecl.Body.ForEach(s => s.Visit(visitor));

            // Create the return type, anonymous by now
            functionSymbol.UpdateReturnType(visitor.Inferrer.NewAnonymousType());

            // Restore previous scope
            visitor.SymbolTable.LeaveScope();
        }

        private void CreateParameterSymbol(SymbolResolverVisitor visitor, FunctionSymbol functionSymbol, ParameterNode parameter)
        {
            // If the parameter's type is present use it, if not use an anonymous type
            var paramTypeInfo = parameter.SymbolInfo.Type == null
                        ? visitor.Inferrer.NewAnonymousType()
                        : new TypeInfo(SymbolHelper.GetType(visitor.SymbolTable, parameter.SymbolInfo.Type));

            // Check if the parameter has storage modifiers
            var storage = SymbolHelper.GetStorage(parameter.SymbolInfo.Mutability);

            // Create the parameter symbol in the function's scope
            functionSymbol.CreateParameter(parameter.Name.Value, paramTypeInfo, storage);
        }
    }
}
