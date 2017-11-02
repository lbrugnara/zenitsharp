// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.StdLib
{
    public class StdLibInitializer
    {
        public static void Import(Scope scope)
        {
            // Namespace: global
            scope.NewSymbol("import", new ImportFunction());
            scope.NewSymbol("using", new UsingFunction());

            // Namespace: sys
            FlNamespace sys = new FlNamespace("sys");

            // Namespace: sys.lang
            FlNamespace lang = new FlNamespace("lang", sys);
            lang["debug"] = new sys.lang.DebugFunction();

            // Namespace: sys.io
            FlNamespace io = new FlNamespace("io", sys);
            io["print"] = new sys.io.PrintFunction();
            io["println"] = new sys.io.PrintLnFunction();

            // Namespace: os
            FlNamespace os = new FlNamespace("os");
            os["cwd"] = new os.CwdFunction();

            // Init
            scope.NewSymbol("sys", sys);
            scope.NewSymbol("os", os);
        }
    }
}
