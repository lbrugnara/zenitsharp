// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Evaluators
{
    class AccessorNodeEvaluator : INodeEvaluator<AstEvaluator, AstAccessorNode, ScopeEntry>
    {
        public ScopeEntry Evaluate(AstEvaluator evaluator, AstAccessorNode invoke)
        {
            ScopeEntry entry = null;
            var self = invoke.Self.Value.ToString();

            // If the identifier exists in the current scope, return it
            if (evaluator.CurrentScope.IsDefined(self, true))
            {
                entry = evaluator.CurrentScope[self];
            }
            else if (invoke.Member == null)
            {
                // If there is no member of "self" it means this (self) symbol doesn't exist
                throw new AstWalkerException($"Symbol '{self}' is not defined");
            }
            else
            {
                // If there is a member, resolve it ...                
                entry = invoke.Member.Exec(evaluator);

                // ... if it is a namespace and "self" is not defined, throw the exception
                if (entry.IsNamespace && entry.NamespaceValue[self] == null)
                    throw new AstWalkerException($"Symbol '{self}' is not defined");
                entry = entry.NamespaceValue[self];                
            }
            return entry;
        }
    }
}
