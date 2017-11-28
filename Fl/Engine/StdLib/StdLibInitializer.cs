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
            scope.AddSymbol("import", new Symbol(SymbolType.Constant), new builtin.ImportFunction());
            scope.AddSymbol("using", new Symbol(SymbolType.Constant), new builtin.UsingFunction());
            scope.AddSymbol("int", new Symbol(SymbolType.Constant), builtin.types.IntegerClass.Build());
            scope.AddSymbol("double", new Symbol(SymbolType.Constant), builtin.types.DoubleClass.Build());
            scope.AddSymbol("decimal", new Symbol(SymbolType.Constant), builtin.types.DecimalClass.Build());
            scope.AddSymbol("bool", new Symbol(SymbolType.Constant), builtin.types.BoolClass.Build());
            scope.AddSymbol("string", new Symbol(SymbolType.Constant), builtin.types.StringClass.Build());
            scope.AddSymbol("Func", new Symbol(SymbolType.Constant), builtin.types.FuncClass.Build());


            // Namespace: std
            FlNamespace std = new FlNamespace("std");
            scope.AddSymbol(std.Name, new Symbol(SymbolType.Constant), std);

            // Namespace: std.lang
            FlNamespace lang = new FlNamespace("lang", std);
            lang.AddSymbol("debug", new Symbol(SymbolType.Constant), new std.lang.DebugFunction());
            lang.AddSymbol("version", new Symbol(SymbolType.Constant), new FlString("alpha 0.0.1"));

            // Namespace: std.io
            FlNamespace io = new FlNamespace("io", std);
            io.AddSymbol("print", new Symbol(SymbolType.Constant), new std.io.PrintFunction());
            io.AddSymbol("println", new Symbol(SymbolType.Constant), new std.io.PrintLnFunction());

            // Namespace: os
            FlNamespace os = new FlNamespace("os");
            scope.AddSymbol(os.Name, new Symbol(SymbolType.Constant), os);
            os.AddSymbol("cwd", new Symbol(SymbolType.Constant), new os.CwdFunction());
        }
    }
}
