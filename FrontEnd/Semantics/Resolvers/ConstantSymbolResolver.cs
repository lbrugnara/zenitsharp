// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Exceptions;
using Zenit.Semantics.Symbols;
using Zenit.Semantics.Symbols;
using Zenit.Semantics.Symbols.Types;
using Zenit.Semantics.Symbols.Types.Primitives;
using Zenit.Semantics.Symbols.Types.Specials;
using Zenit.Semantics.Types;

namespace Zenit.Semantics.Resolvers
{
    class ConstantSymbolResolver : INodeVisitor<SymbolResolverVisitor, ConstantNode, ISymbol>
    {
        public ISymbol Visit(SymbolResolverVisitor binder, ConstantNode constdec)
        {            
            IType typeSymbol = null;

            // Get the constant's type or assume it if not present
            if (constdec.Type != null)
                typeSymbol = SymbolHelper.GetTypeSymbol(binder.SymbolTable, binder.Inferrer, constdec.Type);
            else
                typeSymbol = binder.Inferrer.NewAnonymousType();

            foreach (var definition in constdec.Definitions)
            {
                // Get the identifier name
                var constantName = definition.Left.Value;

                // Check if the symbol is already defined
                if (binder.SymbolTable.HasVariableSymbol(constantName))
                    throw new SymbolException($"Symbol {constantName} is already defined.");

                // If it is a variable definition, visit the right-hand side expression
                var rhsSymbol = definition.Right?.Visit(binder);

                if (rhsSymbol != null && !(rhsSymbol is IPrimitive))
                    throw new SymbolException($"The expression to initialize '{constantName}' must be constant");                

                // Create the new symbol for the variable
                var boundSymbol = binder.SymbolTable.AddNewVariableSymbol(constantName, typeSymbol, Access.Public, Storage.Constant);

                if (typeSymbol is Anonymous asym)
                    binder.Inferrer.TrackSymbol(asym, boundSymbol);
            }

            return null;
        }
    }
}
