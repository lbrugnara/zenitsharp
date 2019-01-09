// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Exceptions;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Inferrers
{
    class AccessorTypeInferrer : INodeVisitor<TypeInferrerVisitor, AccessorNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor inferrer, AccessorNode accessor)
        {
            ISymbol symbol = null;
            string symbolName = accessor.Target.Value;

            // If this is the end of the accessor path, get the symbol in the current
            // scope and return its information
            if (accessor.Parent == null)
            {
                // Get accessed symbol that must be defined in the symtable's scope
                symbol = inferrer.SymbolTable.Get(symbolName);

                // Return the inferred type information for this symbol
                return new InferredType(symbol.TypeInfo, symbol);
            }

            // If the accessed member has an eclosing accessor node, visit
            // it to get the enclosing symbol's type
            var parentSymbol = accessor.Parent.Visit(inferrer).Symbol;

            if (parentSymbol.TypeInfo.IsStructuralType)
            {
                Object memberType = null;
                if (accessor.IsCall)
                    memberType = parentSymbol.TypeInfo.Type.Functions[symbolName];
                else
                    memberType = parentSymbol.TypeInfo.Type.Properties[symbolName];

                return new InferredType(new TypeInfo(memberType));
            }

            if (parentSymbol.TypeInfo.IsPrimitiveType)
            {

            }

            if (parentSymbol.TypeInfo.IsAnonymousType)
            {
                Object memberType = null;

                // We have constraints that need to be added to the type
                if (accessor.IsCall)
                {
                    var memberFunc = new Function();
                    memberFunc.SetReturnType(inferrer.Inferrer.NewAnonymousType().Type);

                    memberType = parentSymbol.TypeInfo.Type.Functions[symbolName] = memberFunc;
                }
                else
                {
                    memberType = parentSymbol.TypeInfo.Type.Properties[symbolName] = inferrer.Inferrer.NewAnonymousType().Type;
                }

                return new InferredType(new TypeInfo(memberType));
            }

            throw new ScopeOperationException($"Member {symbolName} couldn't be retrieved from enclosing symbol {parentSymbol}");
        }
    }
}
