// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Ast;
using Fl.Lang.Types;

namespace Fl.TypeChecking.Checkers
{
    public class CallableTypeChecker : INodeVisitor<TypeCheckerVisitor, AstCallableNode, Type>
    {
        public Type Visit(TypeCheckerVisitor checker, AstCallableNode node)
        {
            Type target = node.Callable.Visit(checker);

            // Generate the "param" instructions
            node.Arguments.Expressions.ForEach(a => a.Visit(checker));

            return target;
        }
    }
}
