// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Evaluators;
using Fl.Engine.Symbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.StdLib.std.lang
{
    public class DebugFunction : FlCallable
    {
        public override string Name => "debug";

        public override FlObject Invoke(AstEvaluator evaluator, List<FlObject> args)
        {
            args.ForEach(a => System.Console.WriteLine(a.IsNamespace ? a.AsNamespace.ShowNamespace() : a.ToDebugStr()));
            return null;
        }
    }
}
