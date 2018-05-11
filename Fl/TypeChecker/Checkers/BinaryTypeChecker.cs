// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;

using Fl.Engine.Symbols.Types;
using Fl.Parser;
using Fl.Ast;

namespace Fl.TypeChecker.Checkers
{
    class BinaryTypeChecker : INodeVisitor<TypeChecker, AstBinaryNode, Symbol>
    {
        public Symbol Visit(TypeChecker checker, AstBinaryNode binary)
        {
            Symbol left = binary.Left.Visit(checker);
            Symbol right = binary.Right.Visit(checker);
            return left;
        }
    }
}
