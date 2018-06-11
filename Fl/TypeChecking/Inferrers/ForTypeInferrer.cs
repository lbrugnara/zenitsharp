// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Ast;
using Fl.Lang.Types;

namespace Fl.TypeChecking.Inferrers
{
    class ForTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstForNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, AstForNode fornode)
        {
            // Create a new block to contain the for's initialization
            visitor.EnterBlock(BlockType.Loop, $"for-{fornode.GetHashCode()}");

            // Initialize the for-block
            fornode.Init.Visit(visitor);

            // Emmit the condition code
            var conditionType = fornode.Condition.Visit(visitor);

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
