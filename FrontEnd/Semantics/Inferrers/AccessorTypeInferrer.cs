﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Exceptions;
using Zenit.Semantics.Symbols.Types;
using Zenit.Semantics.Symbols;
using Zenit.Semantics.Symbols.Types.Specials;
using Zenit.Semantics.Symbols.Variables;
using Zenit.Semantics.Symbols;
using Zenit.Semantics.Symbols.Containers;
using Zenit.Semantics.Symbols.Types.Primitives;
using Zenit.Semantics.Symbols.Types.References;

namespace Zenit.Semantics.Inferrers
{
    class AccessorTypeInferrer : INodeVisitor<TypeInferrerVisitor, AccessorNode, IType>
    {
        public IType Visit(TypeInferrerVisitor inferrer, AccessorNode accessor)
        {           
            string symbolName = accessor.Target.Value;

            // If this is the end of the accessor path, get the symbol in the current
            // scope and return its information
            if (accessor.Parent == null)
            {
                var symbol =
                            // 1- Try to get a bound symbol
                            inferrer.SymbolTable.CurrentScope.TryGet<IVariable>(accessor.Target.Value)
                            // 2- Try to get a type symbol (like functions or objects)
                            ?? inferrer.SymbolTable.CurrentScope.TryGet<IType>(accessor.Target.Value) as ISymbol;

                if (symbol is IVariable bs)                
                    return bs.TypeSymbol;

                return symbol as IType;
            }

            // If the accessed member has an eclosing accessor node, visit
            // it to get the enclosing symbol's type
            var parentSymbol = accessor.Parent.Visit(inferrer);

            if (parentSymbol is IReference cs)
            {
                ISymbol memberType = cs.Get<ISymbol>(symbolName);
                /*if (accessor.IsCall)
                    memberType = cs.Functions[symbolName];
                else
                    memberType = cs.Properties[symbolName];*/

                return memberType is IType mt ? mt : (memberType as IVariable).TypeSymbol;
            }

            if (parentSymbol is IPrimitive)
            {

            }

            if (parentSymbol is Anonymous anons)
            {
                ISymbol memberType = null;

                memberType = /*parentSymbol.TypeSymbol.Type.Properties[symbolName] =*/ inferrer.Inferrer.NewAnonymousType();

                return memberType is IType mt ? mt : (memberType as IVariable).TypeSymbol;
            }

            throw new ScopeOperationException($"Member {symbolName} couldn't be retrieved from enclosing symbol {parentSymbol}");
        }
    }
}
