// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;

using Fl.Engine.Symbols.Types;
using Fl.Ast;

namespace Fl.TypeChecker.Checkers
{
    class ConstantTypeChecker : INodeVisitor<TypeChecker, AstConstantNode, Symbol>
    {
        public Symbol Visit(TypeChecker checker, AstConstantNode constdec)
        {
            // Get the constant's type
            var type = TypeHelper.FromToken(constdec.Type);

            foreach (var declaration in constdec.Constants)
            {
                // Get the identifier name
                var constantName = declaration.Item1.Value.ToString();

                // Get the right-hand side operand (a must for a constant)
                var rhs = declaration.Item2.Visit(checker);

                // const <identifier> = <operand>
                var symbol = checker.SymbolTable.NewSymbol(constantName, type);
            }
            return null;
        }
    }
}
