// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Engine.Symbols.Types;
using Fl.Ast;
using System.Linq;

namespace Fl.TypeChecker.Checkers
{
    class FuncDeclTypeChecker : INodeVisitor<TypeChecker, AstFuncDeclNode, Symbol>
    {
        public Symbol Visit(TypeChecker checker, AstFuncDeclNode funcdecl)
        {
            checker.EnterBlock(BlockType.Function, $"func-{funcdecl.Identifier.Value}-{funcdecl.GetHashCode()}");
            funcdecl.Parameters.Parameters.ForEach(p => checker.SymbolTable.NewSymbol(p.Value.ToString(), null));
            funcdecl.Body.ForEach(s => s.Visit(checker));
            if (!funcdecl.Body.Any(n => n is AstReturnNode))
                funcdecl.Body.OfType<AstReturnNode>().ToList().ForEach(rn => rn.Visit(checker));
            checker.LeaveBlock();

            return null;
        }
    }
}
