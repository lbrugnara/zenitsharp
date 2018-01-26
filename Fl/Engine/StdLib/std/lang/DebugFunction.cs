﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using System.Collections.Generic;

namespace Fl.Engine.StdLib.std.lang
{
    public class DebugFunction : FlFunction
    {
        public override string Name => "debug";

        public override FlObject Invoke(SymbolTable symboltable, List<FlObject> args)
        {
            args.ForEach(a => System.Console.WriteLine(a.ToDebugStr()));
            return null;
        }
    }
}
