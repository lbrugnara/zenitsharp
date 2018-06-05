// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Ast;
using System.Linq;
using Fl.Lang.Types;

namespace Fl.TypeChecking.Inferrers
{
    class FuncDeclTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstFuncDeclNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor checker, AstFuncDeclNode funcdecl)
        {
            checker.EnterBlock(BlockType.Function, $"func-{funcdecl.Identifier.Value}-{funcdecl.GetHashCode()}");


            for (int i=0; i < funcdecl.Parameters.Parameters.Count; i++)
            {
                var p = funcdecl.Parameters.Parameters[i];
                var param = checker.SymbolTable.GetSymbol(p.Value.ToString());

                /*if (param.Type == null)
                    param.Type = new Anonymous($"{(char)('a'+i)}");*/
            }

            var bodyTypes = funcdecl.Body.Select(s => s.Visit(checker)).ToList();


            InferredType returnType = null;

            if (funcdecl.Body.Any(n => n is AstReturnNode))
            {
                var returnTypes = funcdecl.Body.OfType<AstReturnNode>().ToList().Select(rn => rn.Visit(checker));

                if (returnTypes.Distinct().Count() != 1)
                    throw new System.Exception($"Unexpected multiple return types ({string.Join(", ", returnTypes.Distinct())}) in function {funcdecl.Identifier.Value}");

                returnType = returnTypes.First();
            }
            else if (funcdecl.Identifier.Type == Parser.TokenType.RightArrow)
            {
                returnType = bodyTypes.LastOrDefault();
            }

            checker.LeaveBlock();

            return returnType;
        }
    }
}
