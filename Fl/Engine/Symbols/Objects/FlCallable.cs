// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Evaluators;
using Fl.Engine.StdLib;
using Fl.Engine.Symbols.Types;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Symbols
{
    public abstract class FlCallable : FlObject
    {
        public override ObjectType ObjectType => FunctionType.Value;

        public override bool IsPrimitive => true;
        
        public abstract string Name { get; }

        public abstract FlObject Invoke(AstEvaluator evaluator, List<FlObject> args);

        public override object RawValue => Name;

        public override FlObject Clone()
        {
            throw new System.NotImplementedException();
        }
    }
}
