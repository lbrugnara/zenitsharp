// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Symbols.Types;

namespace Fl.TypeChecking.Inferrers
{
    class ContinueTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstContinueNode, Type>
    {
        public Type Visit(TypeInferrerVisitor visitor, AstContinueNode cnode)
        {
            return null;
        }
    }
}
