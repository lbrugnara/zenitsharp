// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;
using System.Collections.Generic;
using System.Linq;

namespace Fl.Semantics.Inferrers
{
    class ClassMethodTypeInferrer : INodeVisitor<TypeInferrerVisitor, ClassMethodNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, ClassMethodNode method)
        {
            // Get the method symbol and type
            var methodSymbol = visitor.SymbolTable.GetSymbol(method.Name);
            Function methodType = methodSymbol.Type as Function;

            // Enter the requested method's block
            var functionScope = visitor.SymbolTable.EnterFunctionScope(method.Name);

            // Grab all the parameters' symbols
            var parametersSymbols = new List<Symbol>();

            parametersSymbols.AddRange(method.Parameters.Select(param => visitor.SymbolTable.GetSymbol(param.Name.Value)));

            // Visit the method's body
            var statements = method.Body.Select(s => (node: s, inferred: s.Visit(visitor))).ToList();

            if (method.IsLambda)
            {
                // If method is a lambda, the return type should be already populated by the lambda's body expression
                // and that should be reflected on the @ret symbol
                var lambdaReturnExpr = statements.Select(s => s.inferred).Last();

                // Try to unify these types
                visitor.Inferrer.InferFromType(lambdaReturnExpr.Type, functionScope.ReturnSymbol.Type);
            }

            // Leave the method's scope
            visitor.SymbolTable.LeaveScope();

            // The inferred method type is a complex type, it might contain assumptions for parameters' types or return type
            // if that is the case, make this inferred type an assumption
            if (visitor.Inferrer.IsTypeAssumption(methodType))
                visitor.Inferrer.AddTypeDependency(methodType, methodSymbol);

            // Return inferred method type
            return new InferredType(methodType, methodSymbol);
        }
    }
}
