// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.StdLib
{
    public class StdLibInitializer
    {
        public static void Import(Scope scope)
        {
            // Namespace: global (built-in)
            scope.AddSymbol("import", new Symbol(StorageType.Constant), new BuiltIn.ImportFunction());
            scope.AddSymbol("using", new Symbol(StorageType.Constant), new BuiltIn.UsingFunction());
            scope.AddSymbol("int", new Symbol(StorageType.Constant), new BuiltIn.CastFunction("int", IntegerType.Value));
            scope.AddSymbol("double", new Symbol(StorageType.Constant), new BuiltIn.CastFunction("double", DoubleType.Value));
            scope.AddSymbol("decimal", new Symbol(StorageType.Constant), new BuiltIn.CastFunction("decimal", DecimalType.Value));
            scope.AddSymbol("str", new Symbol(StorageType.Constant), new BuiltIn.CastFunction("str", StringType.Value));

            // Namespace: std
            FlNamespace std = new FlNamespace("std");
            scope.AddSymbol(std.Name, new Symbol(StorageType.Constant), std);

            // Namespace: std.lang
            FlNamespace lang = new FlNamespace("lang", std);
            lang.AddSymbol("debug", new Symbol(StorageType.Constant), new std.lang.DebugFunction());
            lang.AddSymbol("version", new Symbol(StorageType.Constant), new FlString("alpha 0.0.1"));

            // Namespace: std.io
            FlNamespace io = new FlNamespace("io", std);
            io.AddSymbol("print", new Symbol(StorageType.Constant), new std.io.PrintFunction());
            io.AddSymbol("println", new Symbol(StorageType.Constant), new std.io.PrintLnFunction());

            // Namespace: os
            FlNamespace os = new FlNamespace("os");
            scope.AddSymbol(os.Name, new Symbol(StorageType.Constant), os);
            os.AddSymbol("cwd", new Symbol(StorageType.Constant), new os.CwdFunction());
        }
    }
}
