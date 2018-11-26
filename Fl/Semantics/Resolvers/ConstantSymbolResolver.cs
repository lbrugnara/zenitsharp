// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Resolvers
{
    class ConstantSymbolResolver : INodeVisitor<SymbolResolverVisitor, ConstantNode>
    {
        public void Visit(SymbolResolverVisitor binder, ConstantNode constdec)
        {            
            Object type = null;

            // Get the constant's type or assume it if not present
            if (constdec.Type != null)
                type = SymbolHelper.GetType(binder.SymbolTable, binder.Inferrer, constdec.Type);
            else
                type = binder.Inferrer.NewAnonymousType();

            var typeAssumption = binder.Inferrer.IsTypeAssumption(type);

            foreach (var definition in constdec.Definitions)
            {
                // Get the identifier name
                var constantName = definition.Left.Value;

                // Create the new symbol
                var symbol = binder.SymbolTable.CreateSymbol(constantName, type, Access.Public, Storage.Constant);

                // Register under the assumption of having an anonymous type, if needed
                if (typeAssumption)
                    binder.Inferrer.AddTypeDependency(type, symbol);

                // Get the right-hand side operand (a must for a constant)
                definition.Right.Visit(binder);                

            }
        }
    }
}
