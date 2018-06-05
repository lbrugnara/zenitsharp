// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;
using Fl.Lang.Types;
using Fl.Symbols;

namespace Fl.TypeChecking.Inferrers
{
    class BlockTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstBlockNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor checker, AstBlockNode node)
        {
            checker.EnterBlock(BlockType.Common, $"block-{node.GetHashCode()}");

            foreach (AstNode statement in node.Statements)
                statement.Visit(checker);

            checker.LeaveBlock();
            return null;
        }
    }
}
