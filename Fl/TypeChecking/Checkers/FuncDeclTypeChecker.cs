// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Ast;
using System.Linq;
using Fl.Symbols.Types;

namespace Fl.TypeChecking.Checkers
{
    class FuncDeclTypeChecker : INodeVisitor<TypeCheckerVisitor, AstFuncDeclNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, AstFuncDeclNode funcdecl)
        {
            // TODO
            var funcsym = checker.SymbolTable.GetSymbol(funcdecl.Name);
            return new CheckedType(funcsym.Type, funcsym);
        }
    }
}
