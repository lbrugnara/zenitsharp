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
    class ReturnNodeEvaluator : INodeVisitor<AstEvaluator, AstReturnNode, FlObject>
    {
        public FlObject Visit(AstEvaluator evaluator, AstReturnNode wnode)
        {
            FlObject retval = wnode.ReturnTuple?.Exec(evaluator) ?? FlNull.Value;
            if (retval.ObjectType == TupleType.Value && (retval as FlTuple).Value.Count == 1)
            {
                retval = (retval as FlTuple).Value.ElementAtOrDefault(0);
            }
            evaluator.Symtable.ReturnValue = retval;
            return retval;
        }
    }
}
