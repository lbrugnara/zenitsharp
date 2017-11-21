// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Types;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Evaluators
{
    class AccessorNodeEvaluator : INodeEvaluator<AstEvaluator, AstAccessorNode, FlObject>
    {
        public FlObject Evaluate(AstEvaluator evaluator, AstAccessorNode invoke)
        {
            Symbol entry = null;
            var self = invoke.Self.Value.ToString();

            if (invoke.Member != null) {
                // If there is a member, resolve it ...                
                FlObject ns = invoke.Member.Exec(evaluator);

                // ... if it is a namespace and "self" is not defined, throw the exception
                if (ns.ObjectType == NamespaceType.Value && (ns as FlNamespace)[self] == null)
                    throw new AstWalkerException($"Symbol '{self}' is not defined");
                entry = (ns as FlNamespace)[self];
            }
            else if (evaluator.Symtable.HasSymbol(self)) // If the identifier exists in the current scope, return it
            {
                entry = evaluator.Symtable.GetSymbol(self);
            }
            // If there is no member of "self" it means this (self) symbol doesn't exist
            return entry?.Binding ?? throw new AstWalkerException($"Symbol '{self}' is not defined");
        }

        public Symbol GetSymbol(AstEvaluator evaluator, AstAccessorNode invoke)
        {
            Symbol entry = null;
            var self = invoke.Self.Value.ToString();

            if (invoke.Member != null)
            {
                // If there is a member, resolve it ...                
                FlObject ns = invoke.Member.Exec(evaluator);

                // ... if it is a namespace and "self" is not defined, throw the exception
                if (ns.ObjectType == NamespaceType.Value && (ns as FlNamespace)[self] == null)
                    throw new AstWalkerException($"Symbol '{self}' is not defined");
                entry = (ns as FlNamespace)[self];
            }
            else if (evaluator.Symtable.HasSymbol(self)) // If the identifier exists in the current scope, return it
            {
                entry = evaluator.Symtable.GetSymbol(self);
            }
            // If there is no member of "self" it means this (self) symbol doesn't exist
            return entry ?? throw new AstWalkerException($"Symbol '{self}' is not defined");
        }
    }
}
