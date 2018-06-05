// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Ast;
using Fl.Lang.Types;

namespace Fl.TypeChecking.Inferrers
{
    class WhileTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstWhileNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor checker, AstWhileNode wnode)
        {
            // Generate an eblock instruction for the whole while-block
            checker.EnterBlock(BlockType.Loop, $"while-body-{wnode.GetHashCode()}");

            // Emmit the condition code
            var conditionType = wnode.Condition.Visit(checker);

            // Emmit the body code
            wnode.Body.Visit(checker);

            // Leave the while-block
            checker.LeaveBlock();

            return null;
        }
    }
}
