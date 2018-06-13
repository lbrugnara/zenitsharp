// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Ast;
using Fl.Symbols.Types;

namespace Fl.TypeChecking.Inferrers
{
    class ForTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstForNode, Type>
    {
        public Type Visit(TypeInferrerVisitor visitor, AstForNode fornode)
        {
            // Create a new block to contain the for's initialization
            visitor.EnterBlock(ScopeType.Loop, $"for-{fornode.GetHashCode()}");

            // Initialize the for-block
            fornode.Init.Visit(visitor);

            // Emmit the condition code
            var conditionType = fornode.Condition.Visit(visitor);

            // We know we need a boolean type here
            visitor.Inferrer.MakeConclusion(Bool.Instance, conditionType.DataType);

            // Emmit the body code
            fornode.Body.Visit(visitor);

            // Emmit the for's increment part
            fornode.Increment.Visit(visitor);

            // Leave the for
            visitor.LeaveBlock();

            return null;
        }
    }
}
