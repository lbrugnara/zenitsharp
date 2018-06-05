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
        public InferredType Visit(TypeInferrerVisitor inferrer, AstFuncDeclNode funcdecl)
        {
            // Enter to the function scope and get the @ret symbol

            // If it is a new symbol, update the symbol's type in the symbol table
            string funcName = !funcdecl.IsAnonymous ? funcdecl.Identifier.Value.ToString() : null;

            Function functionSymbol = null;

            // Enter the requested function block if it is a named function. Otherwise access to the block with the UID
            if (funcName != null)
                inferrer.SymbolTable.EnterFunctionBlock(functionSymbol = inferrer.SymbolTable.GetSymbol(funcName) as Function);
            else
                inferrer.EnterBlock(BlockType.Function, $"func-{funcdecl.Identifier.Value}-{funcdecl.GetHashCode()}");


            var retSymbol = inferrer.SymbolTable.CurrentBlock.GetSymbol("@ret");

            // Grab all the parameters' symbols
            var parametersSymbols = new List<Symbol>();

            for (int i=0; i < funcdecl.Parameters.Parameters.Count; i++)
            {
                var p = funcdecl.Parameters.Parameters[i];
                var paramSymbol = inferrer.SymbolTable.GetSymbol(p.Value.ToString());

                // Assign an anonymous type for this parameter
                inferrer.Constraints.AssignTemporalType(paramSymbol);

                parametersSymbols.Add(paramSymbol);
            }

            inferrer.Constraints.AssignTemporalType(retSymbol);

            // Visit the function's body
            var statements = funcdecl.Body.Select(s => (node: s, s.Visit(inferrer))).ToList();

            // Let's infer the function type
            InferredType funcType = null;

            // If there's a lambda, the return type should be already populated by the lambda's body expression
            if (funcdecl.IsLambda)
            {
                var lambdaExpr = statements.Select(s => s.Item2).Last();

                if (retSymbol.Type is Anonymous)
                    inferrer.Constraints.InferTypeAs(retSymbol.Type as Anonymous, lambdaExpr.Type);

                funcType = new InferredType(new Func(parametersSymbols.Select(s => s.Type).ToArray(), retSymbol.Type));
            }
            else
            {
                // If it has a body, get all the AstReturnNode, and check that all the returned types are same (TODO: This needs to get the common ancestor type)
                var returnTypesNode = statements.Where(t => t.node is AstReturnNode).ToList();

                var returnTypes = returnTypesNode.Select(t => t.Item2).Distinct().ToList();

                if (returnTypes.Count() == 1)
                    funcType = new InferredType(new Func(parametersSymbols.Select(s => s.Type).ToArray(), returnTypes.First().Type));
                else if (returnTypes.Count() == 0)
                    funcType = new InferredType(new Func(parametersSymbols.Select(s => s.Type).ToArray(), Null.Instance));
                else
                    throw new System.Exception($"Unexpected multiple return types ({string.Join(", ", returnTypes)}) in function {funcdecl.Identifier.Value}");
            }

            // Double check: Update @ret type
            retSymbol.Type = (funcType.Type as Func).Return;

            // Leave the function's scope
            inferrer.LeaveBlock();

            if (funcName != null)
            {
                functionSymbol.Type = funcType.Type;
                inferrer.Constraints.AddConstraint(functionSymbol);
            }

            return funcType;
        }
    }
}
