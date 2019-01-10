// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Inferrers
{
    class IfTypeInferrer : INodeVisitor<TypeInferrerVisitor, IfNode, ITypeSymbol>
    {
        public ITypeSymbol Visit(TypeInferrerVisitor visitor, IfNode ifnode)
        {
            var conditionType = ifnode.Condition.Visit(visitor);

            // We know we need a boolean type here
            //visitor.Inferrer.ExpectsToUnifyWith(conditionType.TypeSymbol, BuiltinType.Bool);

            // Add a new common block for the if's boyd
            visitor.SymbolTable.EnterBlockScope($"{ifnode.Uid}");

            // Generate the if's body
            ifnode.Then?.Visit(visitor);

            // Leave the if's then block
            visitor.SymbolTable.LeaveScope();

            if (ifnode.Else != null)
            {
                // Add a block for the else's body and generate it, then leave the block
                visitor.SymbolTable.EnterBlockScope($"{ifnode.Uid}");

                ifnode.Else.Visit(visitor);

                visitor.SymbolTable.LeaveScope();
            }

            return null;
        }
    }
}
