// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Evaluators;
using Fl.Engine.StdLib;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Symbols
{
    public abstract class FlCallable : Symbol
    {
        public abstract string Name { get; }

        public FlCallable(StorageType st = StorageType.Constant)
        {
            _DataType = SymbolType.Function;
            _StorageType = st;
            _Value = this;
        }

        public abstract Symbol Invoke(AstEvaluator evaluator, List<Symbol> args);        
    }
}
