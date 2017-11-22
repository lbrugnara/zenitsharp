// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Evaluators;
using System;
using System.Collections.Generic;

namespace Fl.Engine.Symbols.Objects
{
    public class FlMethod: FlCallable
    {
        private string _Name;
        private FlObject _This;
        private Func<FlObject, List<FlObject>, FlObject> _Body;

        public FlMethod(string name, FlObject @this, Func<FlObject, List<FlObject>, FlObject> body)
        {
            _Name = name;
            _This = @this;
            _Body = body;
        }

        public override string Name => _Name;

        public override FlObject Invoke(SymbolTable symboltable, List<FlObject> args)
        {
            return _Body(_This, args);
        }
    }
}
