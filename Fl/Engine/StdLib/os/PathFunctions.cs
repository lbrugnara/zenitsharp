// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Evaluators;
using Fl.Engine.Symbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.StdLib.os
{
    public class CwdFunction : FlCallable
    {
        public override string Name => "cwd";

        public override FlObject Invoke(AstEvaluator evaluator, List<FlObject> args)
        {
            if (args.Count == 0)
                return new FlObject(ObjectType.String, System.IO.Directory.GetCurrentDirectory());
            System.IO.Directory.SetCurrentDirectory(args[0].AsString);
            return args[0];
        }
    }
}
