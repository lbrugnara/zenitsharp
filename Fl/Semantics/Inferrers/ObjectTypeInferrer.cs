// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Inferrers
{
    class ObjectTypeInferrer : INodeVisitor<TypeInferrerVisitor, ObjectNode, ITypeSymbol>
    {
        public ITypeSymbol Visit(TypeInferrerVisitor visitor, ObjectNode node)
        {
            var self = visitor.SymbolTable.EnterObjectScope(node.Uid);
            
            node.Properties.ForEach(p => {
                var propertyInfo = visitor.Visit(p);

                if (propertyInfo is FunctionSymbol funcType)
                {
                    self.Functions[propertyInfo.Name] = funcType;
                }
                else
                {
                    self.Properties[propertyInfo.Name] = propertyInfo;
                }
            });

            visitor.SymbolTable.LeaveScope();

            return self;
        }
    }
}
