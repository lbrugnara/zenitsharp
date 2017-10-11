// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine
{
    public abstract class FlCallable : ScopeEntry
    {
        public abstract string Name { get; }

        public FlCallable()
        {
            _DataType = ScopeEntryType.Function;
            _StorageType = StorageType.Variable;
            _Value = this;
        }

        public abstract ScopeEntry Invoke(AstEvaluator evaluator, List<ScopeEntry> args);        
    }
}
