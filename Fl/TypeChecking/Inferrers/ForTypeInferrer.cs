// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Ast;
using Fl.Lang.Types;

namespace Fl.TypeChecking.Inferrers
{
    class ForTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstForNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor checker, AstForNode fornode)
        {
            // Create a new block to contain the for's initialization
            checker.EnterBlock(BlockType.Loop, $"for-{fornode.GetHashCode()}");

            // Initialize the for-block
            fornode.Init.Visit(checker);

            // Emmit the condition code
            var conditionType = fornode.Condition.Visit(checker);

            // Emmit the body code
            fornode.Body.Visit(checker);

            // Emmit the for's increment part
            fornode.Increment.Visit(checker);

            // Leave the for
            checker.LeaveBlock();

            return null;
        }
    }
}
