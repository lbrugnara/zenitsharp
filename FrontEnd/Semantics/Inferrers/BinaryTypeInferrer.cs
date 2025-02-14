﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Symbols.Types;

namespace Zenit.Semantics.Inferrers
{
    class BinaryTypeInferrer : INodeVisitor<TypeInferrerVisitor, BinaryNode, IType>
    {
        public IType Visit(TypeInferrerVisitor visitor, BinaryNode binary)
        {
            var left = binary.Left.Visit(visitor);
            var right = binary.Right.Visit(visitor);

            return visitor.Inferrer.FindMostGeneralType(left, right);
        }
    }
}
