// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Inferrers
{
    class ObjectTypeInferrer : INodeVisitor<TypeInferrerVisitor, ObjectNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, ObjectNode node)
        {
            var self = visitor.SymbolTable.EnterObjectScope(node.Uid);
            var type = self.TypeInfo.Type;

            node.Properties.ForEach(p => {
                var propertyInfo = visitor.Visit(p);

                if (propertyInfo.TypeInfo.Type is Function funcType)
                {
                    type.Functions[propertyInfo.Symbol.Name] = funcType;
                }
                else
                {
                    type.Properties[propertyInfo.Symbol.Name] = propertyInfo.TypeInfo.Type;
                }
            });

            visitor.SymbolTable.LeaveScope();

            return new InferredType(self.TypeInfo, self);
        }
    }
}
