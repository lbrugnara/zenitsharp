// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Symbols;
using Zenit.Semantics.Symbols.Types.References;
using Zenit.Semantics.Symbols.Variables;

namespace Zenit.Semantics.Checkers
{
    public class CallableTypeChecker : INodeVisitor<TypeCheckerVisitor, CallableNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, CallableNode node)
        {
            var target = node.Target.Visit(checker);

            var targetFuncScope = checker.SymbolTable.CurrentScope.Get<Function>(target.Symbol.Name);

            for (var i=0; i < node.Arguments.Expressions.Count; i++)
            {
                var parameter = targetFuncScope.Get<IVariable>(targetFuncScope.Parameters[i].Name);
                var argument = node.Arguments.Expressions[i];

                var argCheckedType = argument.Visit(checker);

                /*if (!parameter.TypeSymbol.IsAssignableFrom(argCheckedType.TypeSymbol.Type))
                    throw new SymbolException($"Function '{targetFuncScope.Name}' expects parameter '{parameter.Name}' to be of type '{parameter.TypeSymbol}'. Received '{argCheckedType.TypeSymbol}' instead.");*/
            }

            if (target.TypeSymbol is Function f1)
                return new CheckedType(f1.Return.TypeSymbol);

            /*if (target.Type is ClassMethod cm && cm.Type is Function f2)
                return new CheckedType(f2.Return);*/

            /*if (target.ITypeSymbol.Type is Class c)
                return new CheckedType(new ITypeSymbol(new ClassInstance(c)));*/

            throw new System.Exception($"Symbol {target.Symbol.Name} is not a function ({target.TypeSymbol})");
        }
    }
}
