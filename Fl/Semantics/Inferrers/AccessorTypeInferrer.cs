// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Exceptions;
using Fl.Semantics.Symbols;
using Fl.Semantics.Symbols.Values;
using Fl.Semantics.Types;

namespace Fl.Semantics.Inferrers
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
                // Get accessed symbol that must be defined in the symtable's scope
                var symbol = inferrer.SymbolTable.CurrentScope.Get<IValueSymbol>(symbolName);

                if (symbol is IBoundSymbol bs)                
                    return bs.TypeSymbol;

                return symbol as ITypeSymbol;
            }

            // If the accessed member has an eclosing accessor node, visit
            // it to get the enclosing symbol's type
            var parentSymbol = accessor.Parent.Visit(inferrer);

            if (parentSymbol is IComplexSymbol cs)
            {
                IValueSymbol memberType = cs.Get<IValueSymbol>(symbolName);
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
                IValueSymbol memberType = null;

                // We have constraints that need to be added to the type
                if (accessor.IsCall)
                {
                    memberType = /*parentSymbol.Type.Functions[symbolName] =*/ inferrer.Inferrer.NewAnonymousTypeFor();
                }
                else
                {
                    memberType = /*parentSymbol.TypeSymbol.Type.Properties[symbolName] =*/ inferrer.Inferrer.NewAnonymousTypeFor();
                }

                return memberType is ITypeSymbol mt ? mt : (memberType as IBoundSymbol).TypeSymbol;
            }

            throw new ScopeOperationException($"Member {symbolName} couldn't be retrieved from enclosing symbol {parentSymbol}");
        }
    }
}
