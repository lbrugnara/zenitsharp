﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;
using Fl.Semantics.Symbols;

namespace Fl.Semantics.Inferrers
{
    class BlockTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstBlockNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, AstBlockNode node)
        {
            visitor.SymbolTable.EnterScope(ScopeType.Common, $"block-{node.GetHashCode()}");

            foreach (AstNode statement in node.Statements)
                statement.Visit(visitor);

            visitor.SymbolTable.LeaveScope();
            return null;
        }
    }
}