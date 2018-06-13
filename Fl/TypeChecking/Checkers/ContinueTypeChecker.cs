// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Symbols.Types;

namespace Fl.TypeChecking.Checkers
{
    class ContinueTypeChecker : INodeVisitor<TypeCheckerVisitor, AstContinueNode, Type>
    {
        public Type Visit(TypeCheckerVisitor checker, AstContinueNode cnode)
        {
            return Null.Instance;
        }
    }
}
