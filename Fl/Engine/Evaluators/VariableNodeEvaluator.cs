// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Engine.Symbols;
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
            // Get the variable name
            string varname = vardecl.Identifier.Value.ToString();
            // Build the Symbol information from an initializer or use null
            FlObject init = vardecl.Initializer != null ? vardecl.Initializer.Exec(evaluator) : new FlObject(ObjectType.Null, null);
            evaluator.Symtable.AddSymbol(varname, new Symbol(init.Type, StorageType.Variable, init));
            return init;
        }
    }
}
