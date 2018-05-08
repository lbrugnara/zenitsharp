// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;

using Fl.Engine.Symbols.Exceptions;
using Fl.Parser.Ast;

namespace Fl.Symbols.Resolvers
{
    class ReturnSymbolResolver : INodeVisitor<SymbolResolver, AstReturnNode, Symbol>
    {
        public Symbol Visit(SymbolResolver checker, AstReturnNode rnode)
        {
            if (checker.SymbolTable.CurrentBlock.Type != BlockType.Function)
                throw new ScopeOperationException("Invalid return statement in a non-function block");

            return rnode.ReturnTuple?.Visit(checker);
        }
    }
}
