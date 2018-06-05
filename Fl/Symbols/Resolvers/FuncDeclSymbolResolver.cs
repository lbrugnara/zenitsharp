// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using System.Linq;

namespace Fl.Symbols.Resolvers
{
    class FuncDeclSymbolResolver : INodeVisitor<SymbolResolverVisitor, AstFuncDeclNode>
    {
        public void Visit(SymbolResolverVisitor checker, AstFuncDeclNode funcdecl)
        {
            checker.SymbolTable.NewSymbol(funcdecl.Identifier.Value.ToString(), null);

            checker.SymbolTable.EnterBlock(BlockType.Function, $"func-{funcdecl.Identifier.Value}-{funcdecl.GetHashCode()}");

            funcdecl.Parameters.Parameters.ForEach(p => checker.SymbolTable.NewSymbol(p.Value.ToString(), null));

            funcdecl.Body.ForEach(s => s.Visit(checker));

            checker.SymbolTable.LeaveBlock();
        }
    }
}
