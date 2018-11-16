// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Inferrers
{
    class ForTypeInferrer : INodeVisitor<TypeInferrerVisitor, ForNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, ForNode fornode)
        {
            // Create a new block to contain the for's initialization
            visitor.SymbolTable.EnterScope(ScopeType.Loop, $"for-{fornode.GetHashCode()}");

            // Initialize the for-block
            fornode.Init.Visit(visitor);

            // Emmit the condition code
            var conditionType = fornode.Condition.Visit(visitor);

            // We know we need a boolean type here
            visitor.Inferrer.InferFromType(Bool.Instance, conditionType.Type);

            // Emmit the body code
            fornode.Body.Visit(visitor);

            // Emmit the for's increment part
            fornode.Increment.Visit(visitor);

            // Leave the for
            visitor.SymbolTable.LeaveScope();

            return null;
        }
    }
}
