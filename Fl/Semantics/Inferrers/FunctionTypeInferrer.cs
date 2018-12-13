// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;
using System.Collections.Generic;
using System.Linq;

namespace Fl.Semantics.Inferrers
{
    class FunctionTypeInferrer : INodeVisitor<TypeInferrerVisitor, FunctionNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, FunctionNode funcdecl)
        {
            // Get the function symbol
            var functionSymbol = visitor.SymbolTable.GetSymbol(funcdecl.Name);

            // Get the function's type we may update at this step
            Function functionType = functionSymbol.TypeInfo.Type as Function;

            // Enter the requested function's block
            var functionScope = visitor.SymbolTable.EnterFunctionScope(funcdecl.Name);

            // Grab all the parameters' symbols
            var parametersSymbols = new List<Symbol>();

            // TODO: Infer parameter types
            // parametersSymbols.AddRange(funcdecl.Parameters.Select(param => visitor.SymbolTable.GetSymbol(param.Name.Value)));

            // Visit the function's body
            var statements = funcdecl.Body.Select(s => (node: s, inferred: s.Visit(visitor))).ToList();

            // If it is a lambda function, we need to do some "magic" with the return statement, because it is not an explicit
            // statement, we need to get the expression's type (if it is not a void type).
            //  1. Get the expression type
            //  2. Create the return symbol
            //  3. Update its type
            //  4. If the expression type is an assumed type, register that assumption
            if (funcdecl.IsLambda)
            {
                // A lambda function is actually an expression, so, the "last statement" for this
                // type of functions is the "return statement" that defines the lambda's return type 
                // (if the expression is not void)
                var lambdaExpression = statements.Select(s => s.inferred).Last();

                // If the expression has a type, and the return symbol is not defined (by now it MUST NOT be defined at this point)
                // create the @ret symbol and assign the type
                if (lambdaExpression.TypeInfo != null)
                {
                    visitor.Inferrer.Unify(lambdaExpression.TypeInfo, functionScope.ReturnSymbol.TypeInfo);

                    // Update the function's return type with the expression type
                    functionScope.UpdateReturnType(lambdaExpression.TypeInfo);

                    // If the type is an assumed type, register the @ret symbol under that assumption
                    /*if (visitor.Inferrer.IsTypeAssumption(functionScope.ReturnSymbol.TypeInfo))
                        visitor.Inferrer.AddTypeDependency(functionScope.ReturnSymbol.TypeInfo, functionScope.ReturnSymbol);*/
                }           
                else
                {
                    visitor.Inferrer.Unify(Void.Instance, functionScope.ReturnSymbol.TypeInfo);
                }
            }
            else if (!statements.OfType<ReturnNode>().Any() && functionScope.ReturnSymbol.TypeInfo.Type != Void.Instance)
            {
                visitor.Inferrer.Unify(Void.Instance, functionScope.ReturnSymbol.TypeInfo);
            }

            // If the @ret type is an assumption, register the symbol under that assumption too
            /*if (visitor.Inferrer.IsTypeAssumption(functionScope.ReturnSymbol.TypeInfo))
                visitor.Inferrer.AddTypeDependency(functionScope.ReturnSymbol.TypeInfo, functionScope.ReturnSymbol);*/

            // The inferred function type is a complex type, it might contain assumptions for parameters' types or return type
            // if that is the case, make this inferred type an assumption
            /*if (visitor.Inferrer.IsTypeAssumption(functionType))
                visitor.Inferrer.AddTypeDependency(functionType, functionSymbol);*/

            // Leave the function's scope
            visitor.SymbolTable.LeaveScope();

            // Return inferred function type
            return new InferredType(functionSymbol.TypeInfo, functionSymbol);
        }
    }
}
