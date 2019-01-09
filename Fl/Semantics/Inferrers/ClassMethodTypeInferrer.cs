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
            return null;
            // Get the method symbol and type
            /*var methodSymbol = visitor.SymbolTable.GetSymbol(method.Name);
            Function methodType = methodSymbol.TypeInfo as Function;

            // Enter the requested method's block
            var functionScope = visitor.SymbolTable.EnterFunctionScope(method.Name);

            // Grab all the parameters' symbols
            var parametersSymbols = new List<Symbol>();

            // TODO: Infer parameter types
            // parametersSymbols.AddRange(method.Parameters.Select(param => visitor.SymbolTable.GetSymbol(param.Name.Value)));

            // Visit the method's body
            var statements = method.Body.Select(s => (node: s, inferred: s.Visit(visitor))).ToList();

            if (method.IsLambda)
            {
                // If method is a lambda, the return type should be already populated by the lambda's body expression
                // and that should be reflected on the @ret symbol
                var lambdaExpression = statements.Select(s => s.inferred).Last();

                // If the expression has a type, and the return symbol is not defined (by now it MUST NOT be defined at this point)
                // create the @ret symbol and assign the type
                if (lambdaExpression.TypeInfo != null)
                {
                    visitor.Inferrer.Unify(lambdaExpression.TypeInfo, functionScope.ReturnSymbol.TypeInfo);

                    // Update the function's return type with the expression type
                    functionScope.UpdateReturnType(lambdaExpression.TypeInfo);

                    // If the type is an assumed type, register the @ret symbol under that assumption
                    if (visitor.Inferrer.IsTypeAssumption(functionScope.ReturnSymbol.TypeInfo))
                        visitor.Inferrer.AddTypeDependency(functionScope.ReturnSymbol.TypeInfo, functionScope.ReturnSymbol);
                }
                else
                {
                    visitor.Inferrer.Unify(BuiltinType.Void, functionScope.ReturnSymbol.TypeInfo);
                }
            }
            else if (!statements.OfType<ReturnNode>().Any() && functionScope.ReturnSymbol.TypeInfo != BuiltinType.Void)
            {
                visitor.Inferrer.Unify(BuiltinType.Void, functionScope.ReturnSymbol.TypeInfo);
            }

            // If the @ret type is an assumption, register the symbol under that assumption too
            if (visitor.Inferrer.IsTypeAssumption(functionScope.ReturnSymbol.TypeInfo))
                visitor.Inferrer.AddTypeDependency(functionScope.ReturnSymbol.TypeInfo, functionScope.ReturnSymbol);

            // The inferred method type is a complex type, it might contain assumptions for parameters' types or return type
            // if that is the case, make this inferred type an assumption
            if (visitor.Inferrer.IsTypeAssumption(methodType))
                visitor.Inferrer.AddTypeDependency(methodType, methodSymbol);

            // Leave the method's scope
            visitor.SymbolTable.LeaveScope();

            // Return inferred method type
            return new InferredType(methodType, methodSymbol);*/
        }
    }
}
