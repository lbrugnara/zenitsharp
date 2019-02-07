// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Exceptions;
using Zenit.Semantics.Symbols.Types;
using Zenit.Semantics.Symbols;
using Zenit.Semantics.Symbols.Types.Specials;
using Zenit.Semantics.Symbols.Values;
using Zenit.Semantics.Symbols;
using Zenit.Semantics.Symbols.Containers;

namespace Zenit.Semantics.Inferrers
{
    class AccessorTypeInferrer : INodeVisitor<TypeInferrerVisitor, AccessorNode, ITypeSymbol>
    {
        public ITypeSymbol Visit(TypeInferrerVisitor inferrer, AccessorNode accessor)
        {           
            string symbolName = accessor.Target.Value;

            // If this is the end of the accessor path, get the symbol in the current
            // scope and return its information
            if (accessor.Parent == null)
            {
                var symbol =
                            // 1- Try to get a bound symbol
                            inferrer.SymbolTable.CurrentScope.TryGet<IBoundSymbol>(accessor.Target.Value)
                            // 2- Try to get a type symbol (like functions or objects)
                            ?? inferrer.SymbolTable.CurrentScope.TryGet<ITypeSymbol>(accessor.Target.Value) as ISymbol;

                if (symbol is IBoundSymbol bs)                
                    return bs.TypeSymbol;

                return symbol as ITypeSymbol;
            }

            // If the accessed member has an eclosing accessor node, visit
            // it to get the enclosing symbol's type
            var parentSymbol = accessor.Parent.Visit(inferrer);

            if (parentSymbol is IComplexSymbol cs)
            {
                ISymbol memberType = cs.Get<ISymbol>(symbolName);
                /*if (accessor.IsCall)
                    memberType = cs.Functions[symbolName];
                else
                    memberType = cs.Properties[symbolName];*/

                return memberType is ITypeSymbol mt ? mt : (memberType as IBoundSymbol).TypeSymbol;
            }

            if (parentSymbol is IPrimitiveSymbol)
            {

            }

            if (parentSymbol is AnonymousSymbol anons)
            {
                ISymbol memberType = null;

                // We have constraints that need to be added to the type
                if (accessor.IsCall)
                {
                    memberType = /*parentSymbol.Type.Functions[symbolName] =*/ inferrer.Inferrer.NewAnonymousType();
                }
                else
                {
                    memberType = /*parentSymbol.TypeSymbol.Type.Properties[symbolName] =*/ inferrer.Inferrer.NewAnonymousType();
                }

                return memberType is ITypeSymbol mt ? mt : (memberType as IBoundSymbol).TypeSymbol;
            }

            throw new ScopeOperationException($"Member {symbolName} couldn't be retrieved from enclosing symbol {parentSymbol}");
        }
    }
}
