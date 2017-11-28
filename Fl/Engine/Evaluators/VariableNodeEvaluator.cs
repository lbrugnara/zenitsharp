// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
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
            // Get the variable name
            string varname = vardecl.Identifier.Value.ToString();
            // Build the Symbol information from an initializer or use null
            FlObject init = FlNull.Value;
            if (vardecl.Initializer != null)
            {
                init = vardecl.Initializer.Exec(evaluator);
            }
            else if (vardecl.VarType.TypeToken.Type != TokenType.Variable)
            {
                // If it is a typed declaration, run the activator to get the default value
                var clasz = evaluator.Symtable.GetSymbol(vardecl.VarType.TypeToken.Value.ToString())?.Binding as FlClass;
                if (clasz == null)
                    throw new Symbols.Exceptions.SymbolException($"The type {vardecl.VarType.TypeToken.Value} could not be found.");
                init = clasz.Activator.Invoke();
            }
            evaluator.Symtable.AddSymbol(varname, new Symbol(SymbolType.Variable), init);
            return init;
        }
    }
}
