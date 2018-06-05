// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Ast;
using System.Linq;
using Fl.Lang.Types;

namespace Fl.TypeChecking.Checkers
{
    class FuncDeclTypeChecker : INodeVisitor<TypeCheckerVisitor, AstFuncDeclNode, Type>
    {
        public Type Visit(TypeCheckerVisitor checker, AstFuncDeclNode funcdecl)
        {
            checker.EnterBlock(BlockType.Function, $"func-{funcdecl.Identifier.Value}-{funcdecl.GetHashCode()}");

            //funcdecl.Parameters.Parameters.ForEach(p => p);

            funcdecl.Body.ForEach(s => s.Visit(checker));


            Type returnType = null;

            if (funcdecl.Body.Any(n => n is AstReturnNode))
            {
                var returnTypes = funcdecl.Body.OfType<AstReturnNode>().ToList().Select(rn => rn.Visit(checker));

                if (returnTypes.Distinct().Count() != 1)
                    throw new System.Exception($"Unexpected multiple return types ({string.Join(", ", returnTypes.Distinct())}) in function {funcdecl.Identifier.Value}");

                returnType = returnTypes.First();
            }

            checker.LeaveBlock();

            return returnType;
        }
    }
}
