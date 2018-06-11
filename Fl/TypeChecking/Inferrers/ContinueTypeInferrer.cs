﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Lang.Types;

namespace Fl.TypeChecking.Inferrers
{
    class ContinueTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstContinueNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, AstContinueNode cnode)
        {
            return null;
        }
    }
}