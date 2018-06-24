﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Exceptions;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;
using System.Collections.Generic;
using System.Linq;

namespace Fl.Semantics.Inferrers
{
    class FunctionTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstFunctionNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, AstFunctionNode funcdecl)
        {
            var functionSymbol = visitor.SymbolTable.GetSymbol(funcdecl.Name);
            Function functionType = functionSymbol.Type as Function;

            // Enter the requested function's block
            visitor.SymbolTable.EnterScope(ScopeType.Function, funcdecl.Name);

            // Grab all the parameters' symbols
            var parametersSymbols = new List<Symbol>();

            parametersSymbols.AddRange(funcdecl.Parameters.Parameters.Select(param => visitor.SymbolTable.GetSymbol(param.Name.Value.ToString())));

            // Get the return symbol and assign a temporal type
            var retSymbol = visitor.SymbolTable.GetSymbol("@ret");

            // Visit the function's body
            var statements = funcdecl.Body.Select(s => (node: s, inferred: s.Visit(visitor))).ToList();

            if (funcdecl.IsLambda)
            {
                // If function is a lambda, the return type should be already populated by the lambda's body expression
                // and that should be reflected on the @ret symbol
                var lambdaReturnExpr = statements.Select(s => s.inferred).Last();

                if (lambdaReturnExpr.Type is Function f && f.Return == functionType.Return)
                    throw new SymbolException("The function can not be returned to itself");

                // Try to unify these types
                visitor.Inferrer.MakeConclusion(lambdaReturnExpr.Type, retSymbol.Type);
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
