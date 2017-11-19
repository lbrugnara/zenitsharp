// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols;
using Fl.Parser.Ast;

namespace Fl.Engine.Evaluators
{
    class OperandUnaryNode
    {
        private AstAccessorNode GetAccessorNode(AstUnaryNode node)
        {
            var tmp = node;
            while (tmp != null && !(tmp.Left is AstAccessorNode))
            {
                tmp = tmp.Left as AstUnaryNode;
            }
            return tmp?.Left as AstAccessorNode;
        }

        public Symbol GetSymbol(AstEvaluator evaluator, AstUnaryNode node)
        {
            AstAccessorNode accessor = GetAccessorNode(node);
            if (accessor == null)
                return null;

            Symbol entry = null;
            var self = accessor.Self.Value.ToString();

            // If the identifier exists in the current scope, return it
            if (evaluator.Symtable.HasSymbol(self))
            {
                entry = evaluator.Symtable.GetSymbol(self);
            }
            else if (accessor.Member == null)
            {
                // If there is no member of "self" it means this (self) symbol doesn't exist
                throw new AstWalkerException($"Symbol '{self}' is not defined");
            }
            else
            {
                // If there is a member, resolve it ...                
                FlObject ns = accessor.Member.Exec(evaluator);

                // ... if it is a namespace and "self" is not defined, throw the exception
                if (ns.IsNamespace && ns.AsNamespace[self] == null)
                    throw new AstWalkerException($"Symbol '{self}' is not defined");
                entry = ns.AsNamespace[self];
            }
            return entry;
        }
    }
}
