// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Ast;
using Fl.Lang.Types;

namespace Fl.TypeChecking.Inferrers
{
    public class CallableTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstCallableNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor checker, AstCallableNode node)
        {
            var target = node.Callable.Visit(checker);

            // Generate the "param" instructions
            node.Arguments.Expressions.ForEach(a => a.Visit(checker));

            return target;
        }
    }
}
