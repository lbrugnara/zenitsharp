// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using System.Collections.Generic;
using System.Linq;

namespace Fl.Engine.StdLib.builtin
{
    public class ExitFunction : FlFunction
    {
        public override string Name => "exit";

        public override FlObject Invoke(SymbolTable symboltable, List<FlObject> args)
        {
            var exitval = args.ElementAtOrDefault(0) ?? new FlInteger(0);
            symboltable.GlobalScope.AddSymbol("@flexit", new Symbol(SymbolType.Constant), exitval);
            return null;
        }
    }
}
