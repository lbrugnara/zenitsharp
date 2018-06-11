// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Ast;
using Fl.Lang.Types;

namespace Fl.TypeChecking.Inferrers
{
    class WhileTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstWhileNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, AstWhileNode wnode)
        {
            // Generate an eblock instruction for the whole while-block
            visitor.EnterBlock(BlockType.Loop, $"while-body-{wnode.GetHashCode()}");

            // Emmit the condition code
            var conditionType = wnode.Condition.Visit(visitor);

            // Emmit the body code
            wnode.Body.Visit(visitor);

            // Leave the while-block
            visitor.LeaveBlock();

            return null;
        }
    }
}
