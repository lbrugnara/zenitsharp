// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.StdLib.sys.io
{
    public class PrintFunction : FlCallable
    {
        public override string Name => "print";

        public override ScopeEntry Invoke(AstEvaluator evaluator, List<ScopeEntry> args)
        {
            args.ForEach(a => System.Console.Write(a));
            return null;
        }
    }

    public class PrintLnFunction : FlCallable
    {
        public override string Name => "println";

        public override ScopeEntry Invoke(AstEvaluator evaluator, List<ScopeEntry> args)
        {
            args.ForEach(a => System.Console.WriteLine(a));
            return null;
        }
    }
}
