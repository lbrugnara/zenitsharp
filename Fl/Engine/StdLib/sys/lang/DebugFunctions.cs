// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.StdLib.sys.lang
{
    public class DebugFunction : FlCallable
    {
        public override string Name => "debug";

        public override ScopeEntry Invoke(AstEvaluator evaluator, List<ScopeEntry> args)
        {
            args.ForEach(a => System.Console.WriteLine(a.IsNamespace ? a.NamespaceValue.ShowNamespace() : a.ToDebugStr()));
            return null;
        }
    }
}
