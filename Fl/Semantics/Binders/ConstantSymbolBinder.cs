// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Types;

namespace Fl.Semantics.Binders
{
    class ConstantSymbolBinder : INodeVisitor<SymbolBinderVisitor, AstConstantNode>
    {
        public void Visit(SymbolBinderVisitor visitor, AstConstantNode constdec)
        {            
            Type type = null;

            // Get the constant's type or assume it if not present
            if (constdec.Type != null)
                type = TypeHelper.FromToken(constdec.Type) ?? visitor.Inferrer.NewAnonymousType();
            else
                type = visitor.Inferrer.NewAnonymousType();

            var typeAssumption = visitor.Inferrer.IsTypeAssumption(type);

            foreach (var declaration in constdec.Constants)
            {
                // Get the identifier name
                var constantName = declaration.Item1.Value.ToString();                

                // Create the new symbol
                var symbol = visitor.SymbolTable.NewSymbol(constantName, type);

                // Register under the assumption of having an anonymous type, if needed
                if (typeAssumption)
                    visitor.Inferrer.AssumeSymbolTypeAs(symbol, type);

                // Get the right-hand side operand (a must for a constant)
                declaration.Item2.Visit(visitor);                

            }
        }
    }
}
