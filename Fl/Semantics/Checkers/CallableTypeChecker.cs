// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Types;

namespace Fl.Semantics.Checkers
{
    public class CallableTypeChecker : INodeVisitor<TypeCheckerVisitor, CallableNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, CallableNode node)
        {
            var target = node.Target.Visit(checker);

            node.Arguments.Expressions.ForEach(a => a.Visit(checker));

            if (target.Type is Function f1)
                return new CheckedType(f1.Return);

            /*if (target.Type is ClassMethod cm && cm.Type is Function f2)
                return new CheckedType(f2.Return);*/

            if (target.Type is Class c)
                return new CheckedType(new ClassInstance(c));

            throw new System.Exception($"Symbol {target.Symbol.Name} is not a function ({target.Type})");
        }
    }
}
