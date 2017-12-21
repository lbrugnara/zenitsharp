// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using Fl.Engine.Symbols.Types;
using Fl.Engine.Symbols.Exceptions;

namespace Fl.Engine.Symbols.Objects
{
    public class FlMethod: FlFunction
    {
        // Methods are bound at Invoke time
        public FlMethod(string name, FlFunction.BoundFunction body, FlFunction.Contract contract = null)
            : base(name, null, body, contract)
        {
        }

        public override FlObject Invoke(SymbolTable symboltable, List<FlObject> args)
        {
            if (!this.IsBound())
                throw new InvocationException($"Cannot invoke unbound method {Name}");

            return this.Body(This, args);
        }
    }
}
