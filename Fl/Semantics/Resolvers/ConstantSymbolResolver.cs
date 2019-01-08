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
            TypeInfo typeInfo = null;

            // Get the constant's type or assume it if not present
            if (constdec.Type != null)
                typeInfo = new TypeInfo(SymbolHelper.GetType(binder.SymbolTable, binder.Inferrer, constdec.Type));
            else
                typeInfo = binder.Inferrer.NewAnonymousType();

            foreach (var definition in constdec.Definitions)
            {
                // Get the identifier name
                var constantName = definition.Left.Value;

                // Create the new symbol
                var symbol = binder.SymbolTable.Insert(constantName, typeInfo, Access.Public, Storage.Constant);

                // Get the right-hand side operand (a must for a constant)
                definition.Right.Visit(binder);
            }
        }
    }
}
