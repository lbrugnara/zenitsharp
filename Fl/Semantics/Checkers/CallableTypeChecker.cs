// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Exceptions;
using Fl.Semantics.Symbols;
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

                if (!parameter.TypeInfo.Type.IsAssignableFrom(argCheckedType.TypeInfo.Type))
                    throw new SymbolException($"Function '{targetFuncScope.Uid}' expects parameter '{parameter.Name}' to be of type '{parameter.TypeInfo}'. Received '{argCheckedType.TypeInfo}' instead.");
            }

            if (target.TypeInfo.Type is Function f1)
                return new CheckedType(new TypeInfo(f1.Return));

            /*if (target.Type is ClassMethod cm && cm.Type is Function f2)
                return new CheckedType(f2.Return);*/

            if (target.TypeInfo.Type is Class c)
                return new CheckedType(new TypeInfo(new ClassInstance(c)));

            throw new System.Exception($"Symbol {target.Symbol.Name} is not a function ({target.TypeInfo})");
        }
    }
}
