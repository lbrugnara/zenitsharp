// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Exceptions;
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
            Function functionType = functionSymbol.Type as Function;

            // Enter the requested function's block
            var functionScope = visitor.SymbolTable.EnterFunctionScope(funcdecl.Name);

            // Grab all the parameters' symbols
            var parametersSymbols = new List<Symbol>();

            // TODO: Infer parameter types
            // parametersSymbols.AddRange(funcdecl.Parameters.Select(param => visitor.SymbolTable.GetSymbol(param.Name.Value)));

            // Visit the function's body
            var statements = funcdecl.Body.Select(s => (node: s, inferred: s.Visit(visitor))).ToList();

            if (funcdecl.IsLambda)
            {
                // If function is a lambda, the return type should be already populated by the lambda's body expression
                // and that should be reflected on the @ret symbol
                var lambdaReturnExpr = statements.Select(s => s.inferred).Last();

                // Try to unify these types
                visitor.Inferrer.MakeConclusion(lambdaReturnExpr.Type, functionScope.ReturnSymbol.Type);
            }

            // Leave the function's scope
            visitor.SymbolTable.LeaveScope();

            // The inferred function type is a complex type, it might contain assumptions for parameters' types or return type
            // if that is the case, make this inferred type an assumption
            if (visitor.Inferrer.IsTypeAssumption(functionType))
                visitor.Inferrer.AssumeSymbolTypeAs(functionSymbol, functionType);

            // Return inferred function type
            return new InferredType(functionType, functionSymbol);
        }
    }
}
