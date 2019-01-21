// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols.Types;

namespace Fl.Semantics.Inferrers
{
    class ForTypeInferrer : INodeVisitor<TypeInferrerVisitor, ForNode, ITypeSymbol>
    {
        public ITypeSymbol Visit(TypeInferrerVisitor visitor, ForNode fornode)
        {
            // Create a new block to contain the for's initialization
            visitor.SymbolTable.EnterLoopScope(fornode.Uid);

            // Initialize the for-block
            fornode.Init.Visit(visitor);

            // Emmit the condition code
            var conditionType = fornode.Condition.Visit(visitor);

            // We know we need a boolean type here
            //visitor.Inferrer.ExpectsToUnifyWith(conditionType.TypeSymbol, BuiltinType.Bool);

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
