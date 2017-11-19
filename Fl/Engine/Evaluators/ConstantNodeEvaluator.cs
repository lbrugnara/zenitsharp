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
    class ConstantNodeEvaluator : INodeEvaluator<AstEvaluator, AstConstantNode, FlObject>
    {
        public FlObject Evaluate(AstEvaluator evaluator, AstConstantNode constdec)
        {
            string constname = constdec.Identifier.Value.ToString();
            FlObject init = constdec.Initializer.Exec(evaluator);
            init = new FlObject(init.Type, init.Value);
            evaluator.Symtable.AddSymbol(constname, new Symbol(init.Type, StorageType.Constant), init);
            return init;
        }
    }
}
