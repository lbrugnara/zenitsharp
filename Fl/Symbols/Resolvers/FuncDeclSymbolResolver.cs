// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Parser.Ast;
using System.Linq;

namespace Fl.Symbols.Resolvers
{
    class FuncDeclSymbolResolver : INodeVisitor<SymbolResolver, AstFuncDeclNode, Symbol>
    {
        public Symbol Visit(SymbolResolver checker, AstFuncDeclNode funcdecl)
        {
            checker.SymbolTable.EnterBlock(BlockType.Function, $"func-{funcdecl.Identifier.Value}-{funcdecl.GetHashCode()}");

            funcdecl.Parameters.Parameters.ForEach(p => checker.SymbolTable.NewSymbol(p.Value.ToString(), null));

            funcdecl.Body.ForEach(s => s.Visit(checker));

            if (!funcdecl.Body.Any(n => n is AstReturnNode))
                funcdecl.Body.OfType<AstReturnNode>().ToList().ForEach(rn => rn.Visit(checker));

            checker.SymbolTable.LeaveBlock();

            return null;
        }
    }
}
