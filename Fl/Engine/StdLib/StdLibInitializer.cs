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
            scope.AddSymbol("exit", new Symbol(SymbolType.Constant), new builtin.ExitFunction());

            // Basic types
            scope.AddSymbol("object", new Symbol(SymbolType.Constant), FlObjectType.Instance);
            scope.AddSymbol("type", new Symbol(SymbolType.Constant), FlTypeType.Instance);
            scope.AddSymbol("bool", new Symbol(SymbolType.Constant), FlBoolType.Instance);
            scope.AddSymbol("char", new Symbol(SymbolType.Constant), FlCharType.Instance);
            scope.AddSymbol("int", new Symbol(SymbolType.Constant), FlIntType.Instance);
            scope.AddSymbol("float", new Symbol(SymbolType.Constant), FlFloatType.Instance);
            scope.AddSymbol("double", new Symbol(SymbolType.Constant), FlDoubleType.Instance);
            scope.AddSymbol("decimal", new Symbol(SymbolType.Constant), FlDecimalType.Instance);            
            scope.AddSymbol("string", new Symbol(SymbolType.Constant), FlStringType.Instance);
            scope.AddSymbol("tuple", new Symbol(SymbolType.Constant), FlTupleType.Instance);
            scope.AddSymbol("func", new Symbol(SymbolType.Constant), FlFuncType.Instance);
            scope.AddSymbol("Error", new Symbol(SymbolType.Constant), FlErrorType.Instance);


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
