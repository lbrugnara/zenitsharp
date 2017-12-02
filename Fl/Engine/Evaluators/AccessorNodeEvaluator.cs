// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
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

            if (invoke.Member != null)
            {
                // If there is a member, resolve it ...                
                FlObject obj = invoke.Member.Exec(evaluator);

                if (evaluator.Symtable.HasSymbol(obj.ObjectType.ClassName))
                {
                    Symbol clasz = evaluator.Symtable.GetSymbol(obj.ObjectType.ClassName);
                    entry = (clasz.Binding as FlClass)[self];

                    if (entry.StorageType == StorageType.Static)
                        throw new AstWalkerException($"Cannot access static member with an instance reference");
                }
                else
                {
                    entry = obj[self];

                    if (entry.StorageType != StorageType.Static && (obj is FlClass))
                        throw new AstWalkerException($"An object reference is required to access a non-static member");
                }

                if (entry.Binding is FlMethod)
                {
                    return (entry.Binding as FlMethod).Bind(obj);
                }
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
                FlObject obj = invoke.Member.Exec(evaluator);
                entry = obj[self];
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
