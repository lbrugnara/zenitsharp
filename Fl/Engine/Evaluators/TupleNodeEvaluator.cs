// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;
using Fl.Parser.Ast;
using System.Linq;

namespace Fl.Engine.Evaluators
{
    class TupleNodeEvaluator : INodeEvaluator<AstEvaluator, AstTupleNode, FlObject>
    {
        public FlObject Evaluate(AstEvaluator evaluator, AstTupleNode node)
        {
            FlClass clasz = evaluator.Symtable.GetSymbol(TupleType.Value.ClassName).Binding as FlClass;
            FlTuple tuple = clasz.Activator.Invoke() as FlTuple;
            clasz.GetConstructor(0).Bind(tuple).Invoke(evaluator.Symtable, node.Items.Select(i => i.Exec(evaluator)).ToList());
            return tuple;
        }
    }
}
