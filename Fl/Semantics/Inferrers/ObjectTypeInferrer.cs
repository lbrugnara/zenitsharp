﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols.Types;

namespace Fl.Semantics.Inferrers
{
    class ObjectTypeInferrer : INodeVisitor<TypeInferrerVisitor, ObjectNode, ITypeSymbol>
    {
        public ITypeSymbol Visit(TypeInferrerVisitor visitor, ObjectNode node)
        {
            var self = visitor.SymbolTable.EnterObjectScope(node.Uid);
            
            node.Properties.ForEach(p => visitor.Visit(p));

            visitor.SymbolTable.LeaveScope();

            return self;
        }
    }
}
