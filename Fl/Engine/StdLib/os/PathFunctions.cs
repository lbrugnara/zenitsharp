// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.StdLib.os
{
    public class CwdFunction : FlCallable
    {
        public override string Name => "cwd";

        public override ScopeEntry Invoke(AstEvaluator evaluator, List<ScopeEntry> args)
        {
            if (args.Count == 0)
                return new ScopeEntry(ScopeEntryType.String, System.IO.Directory.GetCurrentDirectory());
            System.IO.Directory.SetCurrentDirectory(args[0].StrValue);
            return args[0];
        }
    }
}
