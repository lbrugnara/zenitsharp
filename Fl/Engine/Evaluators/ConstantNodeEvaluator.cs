// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Evaluators
{
    class ConstantNodeEvaluator : INodeEvaluator<AstEvaluator, AstConstantNode, ScopeEntry>
    {
        public ScopeEntry Evaluate(AstEvaluator evaluator, AstConstantNode constdec)
        {
            string constname = constdec.Identifier.Value.ToString();
            ScopeEntry init = constdec.Initializer.Exec(evaluator);
            init = new ScopeEntry(init.DataType, StorageType.Constant, init.Value);
            evaluator.CurrentScope.NewSymbol(constname, init);
            return init;
        }
    }
}
