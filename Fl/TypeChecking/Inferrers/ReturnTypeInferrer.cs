// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Exceptions;
using Fl.Ast;
using Fl.Lang.Types;

namespace Fl.TypeChecking.Inferrers
{
    class ReturnTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstReturnNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor checker, AstReturnNode rnode)
        {
            if (!checker.SymbolTable.CurrentBlock.IsFunction)
                throw new ScopeOperationException("Invalid return statement in a non-function block");

            return rnode.ReturnTuple?.Visit(checker);
        }
    }
}
