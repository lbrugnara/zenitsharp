// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Ast;
using Fl.Symbols.Types;

namespace Fl.TypeChecking.Inferrers
{
    class WhileTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstWhileNode, Type>
    {
        public Type Visit(TypeInferrerVisitor visitor, AstWhileNode wnode)
        {
            // Generate an eblock instruction for the whole while-block
            visitor.EnterBlock(ScopeType.Loop, $"while-body-{wnode.GetHashCode()}");

            // Emmit the condition code
            var conditionType = wnode.Condition.Visit(visitor);

            // We know we need a boolean type here
            visitor.Inferrer.MakeConclusion(Bool.Instance, conditionType.DataType);

            // Emmit the body code
            wnode.Body.Visit(visitor);

            // Leave the while-block
            visitor.LeaveBlock();

            return null;
        }
    }
}
