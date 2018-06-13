// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Symbols.Types;
using System.Linq;

namespace Fl.Symbols.Resolvers
{
    class FuncDeclSymbolResolver : INodeVisitor<SymbolResolverVisitor, AstFuncDeclNode>
    {
        public void Visit(SymbolResolverVisitor visitor, AstFuncDeclNode funcdecl)
        {
            var funcName = funcdecl.Name;
            var funcParams = funcdecl.Parameters.Parameters.Select(p => p.Value.ToString()).ToArray();
            var funcSymbol = new Function(funcName, visitor.SymbolTable.Global, funcParams);

            visitor.SymbolTable.AddSymbol(funcSymbol);

            visitor.SymbolTable.EnterFunctionScope(funcSymbol);

            funcdecl.Parameters.Parameters.ForEach(p => visitor.SymbolTable.NewSymbol(p.Value.ToString(), null));

            funcdecl.Body.ForEach(s => s.Visit(visitor));

            visitor.SymbolTable.LeaveScope();
        }
    }
}
