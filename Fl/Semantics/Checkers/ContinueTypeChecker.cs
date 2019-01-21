﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;

namespace Fl.Semantics.Checkers
{
    class ContinueTypeChecker : INodeVisitor<TypeCheckerVisitor, ContinueNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, ContinueNode cnode)
        {
            return null;
        }
    }
}
