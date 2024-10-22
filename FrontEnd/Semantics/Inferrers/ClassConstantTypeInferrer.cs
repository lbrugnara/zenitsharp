﻿using Zenit.Ast;
using Zenit.Semantics.Symbols.Types;

namespace Zenit.Semantics.Inferrers
{
    class ClassConstantTypeInferrer : INodeVisitor<TypeInferrerVisitor, ClassConstantNode, IType>
    {
        public IType Visit(TypeInferrerVisitor inferrer, ClassConstantNode node)
        {
            // Get the constant symbol
            var constant = inferrer.SymbolTable.GetVariableSymbol(node.Name.Value);

            // Get the inferred type of the right-hand side expression and make the conclusions
            var defIValueSymbol = node.Definition.Visit(inferrer);

            // Use the ClassProperty.Type type in the inference process
            inferrer.Inferrer.FindMostGeneralType(constant.TypeSymbol, defIValueSymbol);

            // TODO: By now return the ClassProperty, as the result does not need to be used,
            // but if in the future we support multiple constant declaration, we need to review
            // this, as we would want the ClassProperty.Type type
            return constant.TypeSymbol;
        }
    }
}
