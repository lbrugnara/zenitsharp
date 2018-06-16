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
            var funcType = new Function(visitor.SymbolTable.Global);
            var funcSymbol = new Symbol(funcdecl.Name, funcType);
            

            visitor.SymbolTable.AddSymbol(funcSymbol);

            visitor.SymbolTable.EnterFunctionScope(funcType);

            funcdecl.Parameters.Parameters.ForEach(p => funcType.DefineParameter(p.Value.ToString(), null));

            funcdecl.Body.ForEach(s => s.Visit(visitor));

            visitor.SymbolTable.LeaveScope();
        }
    }
}
