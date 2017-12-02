// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Evaluators;
using System;
using System.Collections.Generic;
using System.Linq;
using Fl.Engine.Symbols.Types;
using Fl.Engine.Symbols.Exceptions;

namespace Fl.Engine.Symbols.Objects
{
    public class FlMethod: FlFunction
    {
        public FlMethod(string name, Func<FlObject, List<FlObject>, FlObject> body)
            : base(name, body)
        {
        }

        public override FlObject Invoke(SymbolTable symboltable, List<FlObject> args)
        {
            return Body(This ?? throw new InvocationException($"Cannot invoke unbound method {Name}"), args);
        }
    }
}
