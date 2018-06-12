// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;
using Fl.Symbols;

namespace Fl.TypeChecking.Inferrers
{
    class BlockTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstBlockNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, AstBlockNode node)
        {
            visitor.EnterBlock(BlockType.Common, $"block-{node.GetHashCode()}");

            foreach (AstNode statement in node.Statements)
                statement.Visit(visitor);

            visitor.LeaveBlock();
            return null;
        }
    }
}
