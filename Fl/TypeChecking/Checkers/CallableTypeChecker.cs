// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Symbols.Types;

namespace Fl.TypeChecking.Checkers
{
    public class CallableTypeChecker : INodeVisitor<TypeCheckerVisitor, AstCallableNode, SType>
    {
        public SType Visit(TypeCheckerVisitor checker, AstCallableNode node)
        {
            SType target = node.Callable.Visit(checker);

            // Generate the "param" instructions
            node.Arguments.Expressions.ForEach(a => a.Visit(checker));

            return target;
        }
    }
}
