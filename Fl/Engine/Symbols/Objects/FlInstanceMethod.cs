// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;
using Fl.Engine.Symbols.Exceptions;

namespace Fl.Engine.Symbols.Objects
{
    public enum MemberType
    {
        Instance,
        Static
    }

    public abstract class FlMethod : FlFunction
    {
        public abstract MemberType MemberType { get; }

        public FlMethod(string name, FlFunction.BoundFunction body, FlFunction.Contract contract = null)
           : base(name, null, body, contract)
        {
        }

        protected FlMethod(string name, FlObject self, FlFunction.BoundFunction body, FlFunction.Contract contract = null)
            : base(name, self, body, contract)
        {
        }

        public FlMethod(string name, FlFunction.UnboundFunction body, FlFunction.Contract contract = null)
           : base(name, body, contract)
        {
        }
    }

    public class FlInstanceMethod: FlMethod
    {
        public override MemberType MemberType => MemberType.Instance;

        // Methods are bound at Invoke time
        public FlInstanceMethod(string name, FlFunction.BoundFunction body, FlFunction.Contract contract = null)
            : base(name, null, body, contract)
        {
        }

        protected FlInstanceMethod(string name, FlObject self, FlFunction.BoundFunction body, FlFunction.Contract contract = null)
            : base(name, self, body, contract)
        {
        }

        public override FlFunction Bind(FlObject self)
        {
            return new FlInstanceMethod(Name, self, Body, _Contract);
        }

        public override FlObject Invoke(SymbolTable symboltable, List<FlObject> args)
        {
            if (!this.IsBound())
                throw new InvocationException($"Cannot invoke unbound method {Name}");

            if (!(This is FlInstance))
                throw new InvocationException($"An object reference is needed to invoke member '{Name}'");

            return this.Body(This, args);
        }
    }

    public class FlStaticMethod : FlMethod
    {
        public override MemberType MemberType => MemberType.Static;

        // Methods are bound at Invoke time
        public FlStaticMethod(string name, FlFunction.UnboundFunction body, FlFunction.Contract contract = null)
            : base(name, body, contract)
        {
        }

        public override FlFunction Bind(FlObject self)
        {
            if (self is FlInstance)
                throw new InvocationException($"Member '{Name}' cannot be invoked with an instance reference");
            return this;
        }
    }
}
