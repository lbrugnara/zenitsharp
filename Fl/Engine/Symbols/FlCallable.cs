// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Evaluators;
using Fl.Engine.StdLib;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Symbols
{
    public abstract class FlCallable : FlObject
    {
        public abstract string Name { get; }

        public FlCallable()
            : base(ObjectType.Function, null)
        {
            _Value = this;
        }

        public abstract FlObject Invoke(AstEvaluator evaluator, List<FlObject> args);        
    }
}
