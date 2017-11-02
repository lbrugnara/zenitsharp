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
    class ConstantNodeEvaluator : INodeEvaluator<AstEvaluator, AstConstantNode, Symbol>
    {
        public Symbol Evaluate(AstEvaluator evaluator, AstConstantNode constdec)
        {
            string constname = constdec.Identifier.Value.ToString();
            Symbol init = constdec.Initializer.Exec(evaluator);
            init = new Symbol(init.DataType, StorageType.Constant, init.Value);
            evaluator.CurrentScope.NewSymbol(constname, init);
            return init;
        }
    }
}
