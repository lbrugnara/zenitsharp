﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Symbols.Types;
using Zenit.Semantics.Symbols.Types.Specials;
using Zenit.Semantics.Symbols.Variables;
using Zenit.Semantics.Types;
using System.Collections.Generic;
using System.Linq;

namespace Zenit.Semantics.Inferrers
{
    class FunctionTypeInferrer : INodeVisitor<TypeInferrerVisitor, FunctionNode, IType>
    {
        public IType Visit(TypeInferrerVisitor visitor, FunctionNode funcdecl)
        {
            // Get the function symbol
            var functionScope = visitor.SymbolTable.EnterFunctionScope(funcdecl.Name);

            // Grab all the parameters' symbols
            var parametersSymbols = new List<Variable>();

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
                var lambdaExpressionType = statements.Select(s => s.inferred).Last();

                // If the expression has a type, and the return symbol is not defined (by now it MUST NOT be defined at this point)
                // create the @ret symbol and assign the type
                if (lambdaExpressionType != null)
                {
                    var generalType = visitor.Inferrer.FindMostGeneralType(lambdaExpressionType, functionScope.Return.TypeSymbol);

                    visitor.Inferrer.Unify(visitor.SymbolTable, generalType, functionScope.Return);
                }           
                else
                {
                    visitor.Inferrer.Unify(visitor.SymbolTable, new Void(), functionScope.Return);
                }
            }
            else if (!statements.OfType<ReturnNode>().Any() && functionScope.Return.TypeSymbol.BuiltinType != BuiltinType.Void)
            {
                visitor.Inferrer.Unify(visitor.SymbolTable, new Void(), functionScope.Return);
            }

            // Leave the function's scope
            visitor.SymbolTable.LeaveScope();

            // Return inferred function type
            return functionScope;
        }
    }
}
