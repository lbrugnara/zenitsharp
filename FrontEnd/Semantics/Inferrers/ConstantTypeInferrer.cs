﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Symbols.Types;

namespace Zenit.Semantics.Inferrers
{
    class ConstantTypeInferrer : INodeVisitor<TypeInferrerVisitor, ConstantNode, IType>
    {
        public IType Visit(TypeInferrerVisitor visitor, ConstantNode constdec)
        {
            IType typeInfo = null;

            foreach (var definition in constdec.Definitions)
            {
                // Multiple constant definitions in the same statement are all of the same type so take the first
                if (typeInfo == null)
                    typeInfo = visitor.SymbolTable.GetVariableSymbol(definition.Left.Value).TypeSymbol;

                // Get the right-hand side expression-s type (a must for a constant)
                var rhs = definition.Right.Visit(visitor);

                typeInfo = visitor.Inferrer.FindMostGeneralType(typeInfo, rhs);
            }

            return typeInfo;
        }
    }
}
