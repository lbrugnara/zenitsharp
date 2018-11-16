// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Inferrers
{
    class IfTypeInferrer : INodeVisitor<TypeInferrerVisitor, IfNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, IfNode ifnode)
        {
            var conditionType = ifnode.Condition.Visit(visitor);

            // We know we need a boolean type here
            visitor.Inferrer.InferFromType(Bool.Instance, conditionType.Type);

            // Add a new common block for the if's boyd
            visitor.SymbolTable.EnterScope(ScopeType.Common, $"if-then-{ifnode.GetHashCode()}");

            // Generate the if's body
            ifnode.Then?.Visit(visitor);

            // Leave the if's then block
            visitor.SymbolTable.LeaveScope();

            if (ifnode.Else != null)
            {
                // Add a block for the else's body and generate it, then leave the block
                visitor.SymbolTable.EnterScope(ScopeType.Common, $"if-else-{ifnode.GetHashCode()}");

                ifnode.Else.Visit(visitor);

                visitor.SymbolTable.LeaveScope();
            }

            return null;
        }
    }
}
