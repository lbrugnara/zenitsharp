// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Objects;

namespace Fl.Engine.StdLib.std.io
{
    public class PrintFunction : FlFunction
    {
        public PrintFunction()
            : base("print", (args) => { args.ForEach(a => System.Console.Write(a)); return FlNull.Value; })
        {
        }
    }

    public class PrintLnFunction : FlFunction
    {
        public PrintLnFunction()
            : base("println", (args) => { args.ForEach(a => System.Console.WriteLine(a)); return FlNull.Value; })
        {
        }
    }
}
