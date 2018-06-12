// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Ast;
using System.Linq;
using Fl.Lang.Types;
using System.Collections.Generic;

namespace Fl.TypeChecking.Inferrers
{
    class FuncDeclTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstFuncDeclNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, AstFuncDeclNode funcdecl)
        {
            Function functionSymbol = visitor.SymbolTable.GetSymbol(funcdecl.Name) as Function ?? throw new System.Exception($"Function {funcdecl.Name} has not been resolved");

            // Enter the requested function's block
            visitor.SymbolTable.EnterFunctionBlock(functionSymbol);

            // Grab all the parameters' symbols
            var parametersSymbols = new List<Symbol>();

            for (int i=0; i < funcdecl.Parameters.Parameters.Count; i++)
            {
                var paramSymbol = visitor.SymbolTable.GetSymbol(funcdecl.Parameters.Parameters[i].Value.ToString());

                // If parameter doesn't have a type, assume it
                if (paramSymbol.Type == null)
                    visitor.Inferrer.AssumeSymbolType(paramSymbol);

                parametersSymbols.Add(paramSymbol);
            }

            // Get the return symbol and assign a temporal type
            var retSymbol = visitor.SymbolTable.CurrentBlock.GetSymbol("@ret");

            // If return type is not yet inferred, assume one
            if (retSymbol.Type == null)
                visitor.Inferrer.AssumeSymbolType(retSymbol);

            // Visit the function's body
            var statements = funcdecl.Body.Select(s => (node: s, inferred: s.Visit(visitor))).ToList();

            // Let's infer the function type
            InferredType inferredFuncType = null;

            if (funcdecl.IsLambda)
            {
                // If there's a lambda, the return type should be already populated by the lambda's body expression
                // and that should be reflected on the @ret symbol
                var lambdaReturnExpr = statements.Select(s => s.inferred).Last();

                // Try to unify these types
                visitor.Inferrer.MakeConclusion(retSymbol.Type, lambdaReturnExpr.Type);

                // The inferred Func type with the paremters type and the return type
                inferredFuncType = new InferredType(new Func(parametersSymbols.Select(s => s.Type).ToArray(), retSymbol.Type), functionSymbol);
            }
            else
            {
                // If it has a body, get all the AstReturnNode, and check that all the returned types are same
                // TODO: This needs to get the common ancestor type (union)
                var returnTypesNode = statements.Where(t => t.node is AstReturnNode).ToList();

                var returnTypes = returnTypesNode.Select(t => t.inferred).Distinct().ToList();

                if (returnTypes.Count() == 1)
                {
                    visitor.Inferrer.MakeConclusion(retSymbol.Type, returnTypes.First().Type);
                    inferredFuncType = new InferredType(new Func(parametersSymbols.Select(s => s.Type).ToArray(), retSymbol.Type), functionSymbol);
                }
                else if (returnTypes.Count() == 0)
                {
                    visitor.Inferrer.MakeConclusion(retSymbol.Type, Null.Instance);
                    inferredFuncType = new InferredType(new Func(parametersSymbols.Select(s => s.Type).ToArray(), retSymbol.Type), functionSymbol);
                }
                else
                {
                    throw new System.Exception($"Unexpected multiple return types ({string.Join(", ", returnTypes)}) in function {funcdecl.Name}");
                }
            }

            // Leave the function's scope
            visitor.LeaveBlock();

            // Symbol types is the inferred type
            functionSymbol.Type = inferredFuncType.Type;

            // The inferred function type is a complex type, it might contain assumptions for parameters' types or return type
            // if that is the case, make this inferred type an assumption
            if (visitor.Inferrer.TypeIsAssumed(inferredFuncType.Type))
                visitor.Inferrer.AssumeSymbolTypeAs(functionSymbol, inferredFuncType.Type);

            // Return inferred function type
            return inferredFuncType;
        }
    }
}
