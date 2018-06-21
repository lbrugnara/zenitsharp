// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics;
using Fl.Ast;
using System.Linq;
using Fl.Semantics.Types;

namespace Fl.Semantics.Checkers
{
    class FunctionTypeChecker : INodeVisitor<TypeCheckerVisitor, AstFunctionNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, AstFunctionNode funcdecl)
        {
            // TODO
            var funcsym = checker.SymbolTable.GetSymbol(funcdecl.Name);
            return new CheckedType(funcsym.Type, funcsym);
        }
    }
}
