// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Symbols;
using Fl.Lang.Types;

namespace Fl.TypeChecking.Inferrers
{
    class AccessorTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstAccessorNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor checker, AstAccessorNode accessor)
        {
            if (accessor.Enclosing != null)
                return accessor.Enclosing?.Visit(checker);

            var symbol = checker.SymbolTable.GetSymbol(accessor.Identifier.Value.ToString());

            return new InferredType
            {
                Symbol = symbol,
                Type = symbol.Type
            };
        }
    }
}
