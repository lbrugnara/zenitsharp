// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Inferrers
{
    class WhileTypeInferrer : INodeVisitor<TypeInferrerVisitor, WhileNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, WhileNode wnode)
        {
            // Generate an eblock instruction for the whole while-block
            visitor.SymbolTable.EnterLoopScope($"{wnode.Uid}");

            // Emmit the condition code
            var conditionType = wnode.Condition.Visit(visitor);

            // We know we need a boolean type here
            visitor.Inferrer.Unify(Bool.Instance, conditionType.TypeInfo);

            // Emmit the body code
            wnode.Body.Visit(visitor);

            // Leave the while-block
            visitor.SymbolTable.LeaveScope();

            return null;
        }
    }
}
