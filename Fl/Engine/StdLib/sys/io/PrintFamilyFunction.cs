// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Evaluators;
using Fl.Engine.Symbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.StdLib.sys.io
{
    public class PrintFunction : FlCallable
    {
        public override string Name => "print";

        public override Symbol Invoke(AstEvaluator evaluator, List<Symbol> args)
        {
            args.ForEach(a => System.Console.Write(a));
            return null;
        }
    }

    public class PrintLnFunction : FlCallable
    {
        public override string Name => "println";

        public override Symbol Invoke(AstEvaluator evaluator, List<Symbol> args)
        {
            args.ForEach(a => System.Console.WriteLine(a));
            return null;
        }
    }
}
