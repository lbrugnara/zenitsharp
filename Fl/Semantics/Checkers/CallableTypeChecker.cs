// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Types;

namespace Fl.Semantics.Checkers
{
    public class CallableTypeChecker : INodeVisitor<TypeCheckerVisitor, AstCallableNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, AstCallableNode node)
        {
            var target = node.Callable.Visit(checker);

            node.Arguments.Expressions.ForEach(a => a.Visit(checker));

            if (!(target.Type is Function))
                throw new System.Exception($"Symbol {target.Symbol.Name} is not a function ({target.Type})");

            return new CheckedType((target.Type as Function).Return);
        }
    }
}
