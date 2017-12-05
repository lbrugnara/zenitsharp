// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fl.Engine.Evaluators
{
    class VariableNodeEvaluator : INodeEvaluator<AstEvaluator, AstVariableNode, FlObject>
    {
        public FlObject Evaluate(AstEvaluator evaluator, AstVariableNode vardecl)
        {
            // Get the variable type
            var type = vardecl.VarType;

            // attributes
            bool isArray = type.Dimensions?.Count > 0; // By now allow 1-dimension arrays
            Symbol varsymbol = null;

            foreach (var tuple in vardecl.Variables)
            {
                var id = tuple.Item1;
                var initializer = tuple.Item2;

                // Ignore empty items, it could be a declaration of type var (a,,c) = (1,2,3)
                if (id == null)
                    continue;

                // Get the variable name                
                string varname = id.Value.ToString();

                // Build the Symbol information from an initializer or use null
                varsymbol = new Symbol(SymbolType.Variable);
                FlObject init = FlNull.Value;

                // Initialize to null
                evaluator.Symtable.AddSymbol(varname, varsymbol, FlNull.Value);

                // If type is not null, get the default value of the type and update the binding
                if (type != null && type.TypeToken.Type != TokenType.Variable)
                {
                    var clasz = evaluator.Symtable.GetSymbol(vardecl.VarType.TypeToken.Value.ToString()).Binding as FlClass;
                    FlObject defaultVal = clasz.Activator.Invoke();
                    varsymbol.UpdateBinding(defaultVal);
                }

                // If the declaration has a definition, update the binding. This will raise an exception if types do not match
                if (initializer != null)
                {
                    varsymbol.UpdateBinding(initializer.Exec(evaluator));
                }
            }
            return FlNull.Value;
        }
    }
}
