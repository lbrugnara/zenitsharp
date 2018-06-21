// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics;
using Fl.Ast;
using System.Linq;
using Fl.Semantics.Types;

namespace Fl.Semantics.Checkers
{
    class ClassMethodTypeChecker : INodeVisitor<TypeCheckerVisitor, AstClassMethodNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, AstClassMethodNode method)
        {
            // TODO
            var funcsym = checker.SymbolTable.GetSymbol(method.Name);
            return new CheckedType(funcsym.Type, funcsym);
        }
    }
}
