// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Exceptions;
using Fl.Semantics.Symbols;
using Fl.Semantics.Symbols.Values;
using Fl.Semantics.Types;

namespace Fl.Semantics.Resolvers
{
    class ConstantSymbolResolver : INodeVisitor<SymbolResolverVisitor, ConstantNode, IValueSymbol>
    {
        public IValueSymbol Visit(SymbolResolverVisitor binder, ConstantNode constdec)
        {            
            ITypeSymbol typeSymbol = null;

            // Get the constant's type or assume it if not present
            if (constdec.Type != null)
                typeSymbol = SymbolHelper.GetTypeSymbol(binder.SymbolTable, binder.Inferrer, constdec.Type);
            else
                typeSymbol = binder.Inferrer.NewAnonymousTypeFor();

            foreach (var definition in constdec.Definitions)
            {
                // Get the identifier name
                var constantName = definition.Left.Value;

                // Check if the symbol is already defined
                if (binder.SymbolTable.Contains(constantName))
                    throw new SymbolException($"Symbol {constantName} is already defined.");

                // If it is a variable definition, visit the right-hand side expression
                var rhsSymbol = definition.Right?.Visit(binder);

                if (rhsSymbol != null && !(rhsSymbol is IPrimitiveSymbol))
                    throw new SymbolException($"The expression to initialize '{constantName}' must be a constant expression");                

                // Create the new symbol for the variable
                binder.SymbolTable.Insert(constantName, typeSymbol, Access.Public, Storage.Constant);
            }

            return null;
        }
    }
}
