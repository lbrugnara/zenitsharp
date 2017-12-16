// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Evaluators
{
    class IfNodeEvaluator : INodeVisitor<AstEvaluator, AstIfNode, FlObject>
    {
        public FlObject Visit(AstEvaluator evaluator, AstIfNode ifnode)
        {
            evaluator.Symtable.EnterScope(ScopeType.Common);
            try
            {
                FlObject result = ifnode.Condition.Exec(evaluator);
                if (result.ObjectType != BoolType.Value)
                    throw new AstWalkerException($"Cannot convert type {result.ObjectType} to {BoolType.Value}");
                if ((result as FlBool).Value)
                    return ifnode.Then?.Exec(evaluator);
                if (ifnode.Else != null)
                    return ifnode.Else.Exec(evaluator);
            }
            finally
            {
                evaluator.Symtable.LeaveScope();
            }
            return null;
        }
    }
}
