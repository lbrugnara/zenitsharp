// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;

namespace Fl.Symbols.Resolvers
{
    class ConstantSymbolResolver : INodeVisitor<SymbolResolverVisitor, AstConstantNode>
    {
        public void Visit(SymbolResolverVisitor checker, AstConstantNode constdec)
        {
            // Get the constant's type
            var type = TypeHelper.FromToken(constdec.Type);

            foreach (var declaration in constdec.Constants)
            {
                // Get the identifier name
                var constantName = declaration.Item1.Value.ToString();                

                // const <identifier> = <operand>
                var symbol = checker.SymbolTable.NewSymbol(constantName, type);

                // Get the right-hand side operand (a must for a constant)
                declaration.Item2.Visit(checker);                

            }
        }
    }
}
