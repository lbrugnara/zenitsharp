// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Ast;
using Fl.Symbols.Types;

namespace Fl.TypeChecking.Inferrers
{
    class IfTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstIfNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, AstIfNode ifnode)
        {
            var conditionType = ifnode.Condition.Visit(visitor);

            // We know we need a boolean type here
            visitor.Inferrer.MakeConclusion(Bool.Instance, conditionType.Type);

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
