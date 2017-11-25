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
    class ConstantNodeEvaluator : INodeEvaluator<AstEvaluator, AstConstantNode, FlObject>
    {
        public FlObject Evaluate(AstEvaluator evaluator, AstConstantNode constdec)
        {
            string constname = constdec.Identifier.Value.ToString();
            FlObject init = constdec.Initializer.Exec(evaluator);
            evaluator.Symtable.AddSymbol(constname, new Symbol(SymbolType.Constant), init);
            return init;
        }
    }
}
