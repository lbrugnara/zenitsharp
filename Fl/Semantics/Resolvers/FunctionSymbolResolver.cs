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
            // There are 3 components in the function definition:
            //  Function's type: It is the concrete type that we need to "build" while resolving the symbol. It contains structural information
            //  Function's symbol: Represents the binding of the function to the current scope
            //  Function's scope: The function's symbol contains information about the binding and the type, we also need an scope to keep the function's body
            // The symbol and the type are related and saved in the current scope while the FunctionScope is part of the symbol table (a nested scope)

            // Create the new function type
            var functionType = new Function();

            // Create the function symbol
            var functionSymbol = new Symbol(funcdecl.Name, functionType, Access.Public, Storage.Constant);

            // Register it in the current scope
            visitor.SymbolTable.AddSymbol(functionSymbol);

            // Change the current scope to be the function's scope
            var functionScope = visitor.SymbolTable.EnterFunctionScope(funcdecl.Name);

            // Process the parameters
            funcdecl.Parameters.ForEach(parameter => {
                // Define the symbol in the current scope (function's scope)
                var type = parameter.SymbolInfo.Type == null 
                            ? visitor.Inferrer.NewAnonymousType() 
                            : SymbolHelper.GetType(visitor.SymbolTable, parameter.SymbolInfo.Type);

                // Update the function's type (with parameter type)
                functionType.DefineParameterType(type);

                // Update function's scope with the parameter definition
                var storage = SymbolHelper.GetStorage(parameter.SymbolInfo.Mutability);
                var symbol = functionScope.CreateParameter(parameter.Name.Value, type, Access.Public, storage);

                // Update parameter-anonymous type if the type is an assumed type
                if (visitor.Inferrer.IsTypeAssumption(type))
                    visitor.Inferrer.AssumeSymbolTypeAs(symbol, type);
            });

            // Visit the function's body
            funcdecl.Body.ForEach(s => s.Visit(visitor));

            // At this point, the function's type is an assumed type, register
            // the function's symbol under that assumption
            visitor.Inferrer.AssumeSymbolTypeAs(functionSymbol, functionType);

            // Restore previous scope
            visitor.SymbolTable.LeaveScope();
        }
    }
}
