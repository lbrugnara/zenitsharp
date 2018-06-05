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
            var funcSymbol = new Function(funcdecl.Identifier.Value.ToString(), funcdecl.Parameters.Parameters.Select(p => p.Value.ToString()).ToArray());
            
            // Register symbol if it is not an anontmous function
            if (!funcdecl.IsAnonymous)
                checker.SymbolTable.AddSymbol(funcSymbol);

            checker.SymbolTable.EnterFunctionBlock(funcSymbol);

            funcdecl.Parameters.Parameters.ForEach(p => checker.SymbolTable.NewSymbol(p.Value.ToString(), null));

            funcdecl.Body.ForEach(s => s.Visit(checker));

            checker.SymbolTable.LeaveBlock();
        }
    }
}
