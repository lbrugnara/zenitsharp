// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;

using Fl.Engine.Symbols.Types;
using Fl.Parser.Ast;
using System.Collections.Generic;
using System.Linq;

namespace Fl.TypeChecker.Checkers
{
    public class CallableTypeChecker : INodeVisitor<TypeChecker, AstCallableNode, Symbol>
    {
        public Symbol Visit(TypeChecker checker, AstCallableNode node)
        {
            Symbol target = node.Callable.Visit(checker);

            // Generate the "param" instructions
            node.Arguments.Expressions.ForEach(a => a.Visit(checker));

            return target;
        }
    }
}
