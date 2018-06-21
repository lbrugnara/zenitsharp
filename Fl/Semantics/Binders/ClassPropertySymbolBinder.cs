using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;
using System;

namespace Fl.Semantics.Binders
{
    class ClassPropertySymbolBinder : INodeVisitor<SymbolBinderVisitor, AstClassPropertyNode>
    {
        public void Visit(SymbolBinderVisitor binder, AstClassPropertyNode node)
        {
			// TODO: Implement ClassProperty binder
        }
    }
}
