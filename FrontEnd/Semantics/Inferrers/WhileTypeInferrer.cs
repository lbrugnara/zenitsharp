﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Symbols.Types;

namespace Zenit.Semantics.Inferrers
{
    class WhileTypeInferrer : INodeVisitor<TypeInferrerVisitor, WhileNode, IType>
    {
        public IType Visit(TypeInferrerVisitor visitor, WhileNode wnode)
        {
            // Generate an eblock instruction for the whole while-block
            visitor.SymbolTable.EnterLoopScope($"{wnode.Uid}");

            // Emmit the condition code
            var conditionType = wnode.Condition.Visit(visitor);

            // We know we need a boolean type here
            //visitor.Inferrer.ExpectsToUnifyWith(conditionType.TypeSymbol, BuiltinType.Bool);

            // Emmit the body code
            wnode.Body.Visit(visitor);

            // Leave the while-block
            visitor.SymbolTable.LeaveScope();

            return null;
        }
    }
}
