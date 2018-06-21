using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;
using System;

namespace Fl.Semantics.Binders
{
    class ClassConstantSymbolBinder : INodeVisitor<SymbolBinderVisitor, AstClassConstantNode>
    {
        public void Visit(SymbolBinderVisitor binder, AstClassConstantNode node)
        {
			// TODO: Implement ClassConstant binder
        }
    }
}
