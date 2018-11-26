// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Exceptions;
using Fl.Semantics.Types;

namespace Fl.Semantics.Checkers
{
    public class CallableTypeChecker : INodeVisitor<TypeCheckerVisitor, CallableNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, CallableNode node)
        {
            var target = node.Target.Visit(checker);

            var targetFuncScope = checker.SymbolTable.GetFunctionScope(target.Symbol.Name);

            for (var i=0; i < node.Arguments.Expressions.Count; i++)
            {
                var parameter = targetFuncScope.GetSymbol(targetFuncScope.Parameters[i]);
                var argument = node.Arguments.Expressions[i];

                var argCheckedType = argument.Visit(checker);

                if (!parameter.Type.IsAssignableFrom(argCheckedType.Type))
                    throw new SymbolException($"Function '{targetFuncScope.Uid}' expects parameter '{parameter.Name}' to be of type '{parameter.Type}'. Received '{argCheckedType.Type}' instead.");
            }

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
