// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Symbols.Types;

namespace Fl.TypeChecking.Checkers
{
    class ContinueTypeChecker : INodeVisitor<TypeCheckerVisitor, AstContinueNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, AstContinueNode cnode)
        {
            return null;
        }
    }
}
