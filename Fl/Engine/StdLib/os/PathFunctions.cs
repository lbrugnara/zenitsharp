﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using System.Collections.Generic;

namespace Fl.Engine.StdLib.os
{
    public class CwdFunction : FlFunction
    {
        public override string Name => "cwd";

        public override FlObject Invoke(SymbolTable symboltable, List<FlObject> args)
        {
            if (args.Count == 0)
                return new FlString(System.IO.Directory.GetCurrentDirectory());
            var dir = (args[0] as FlString);
            System.IO.Directory.SetCurrentDirectory(dir.Value);
            return dir;
        }
    }
}
