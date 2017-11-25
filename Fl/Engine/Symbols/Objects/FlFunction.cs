// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Evaluators;
using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fl.Engine.Symbols.Objects
{
    public class FlFunction: FlObject
    {
        private string _Name;
        private FlObject _This;
        private Func<FlObject, List<FlObject>, FlObject> _Body;

        protected FlFunction() { }

        /// <summary>
        /// Body is an unbound function that receives a list of arguments
        /// and returns an FlObject
        /// </summary>
        /// <param name="name">Function name</param>
        /// <param name="body">Lambda that receives a list of FlObjects and returns an FlObject</param>
        public FlFunction(string name, Func<List<FlObject>, FlObject> body)
        {
            _Name = name ?? "anonymous";
            _This = null;
            _Body = (self, args) => body(args);
        }

        /// <summary>
        /// Body is an unbound function that receives a list of arguments along with an object that represents
        /// the this pointer if it is bound using the Bind method
        /// and returns an FlObject
        /// </summary>
        /// <param name="name">Function name</param>
        /// <param name="body">Lambda that receives a list of FlObjects and returns an FlObject</param>
        public FlFunction(string name, Func<FlObject, List<FlObject>, FlObject> body)
        {
            _Name = name ?? "anonymous";
            _This = null;
            _Body = body;
        }

        /// <summary>
        /// Body is an bound function that receives a list of arguments along with an object that represents
        /// the this pointer that will be the value referenced by self
        /// and returns an FlObject
        /// </summary>
        /// <param name="name">Function name</param>
        /// <param name="self">Function name</param>
        /// <param name="body">Lambda that receives a list of FlObjects and returns an FlObject</param>
        protected FlFunction(string name, FlObject self, Func<FlObject, List<FlObject>, FlObject> body)
        {
            _Name = name ?? "anonymous";
            _This = self;
            _Body = body;
        }

        protected virtual FlObject This => _This;

        protected virtual Func<FlObject, List<FlObject>, FlObject> Body => _Body;

        public virtual string Name => _Name;

        public override object RawValue => Body;

        public override bool IsPrimitive => true;

        public override ObjectType ObjectType => FuncType.Value;

        public override FlObject Clone()
        {
            return new FlFunction(Name, Body);
        }

        public override string ToString()
        {
            return Name;
        }

        public override string ToDebugStr()
        {
            return $"{Name} (func)";
        }

        public override FlObject ConvertTo(ObjectType type)
        {
            if (type == FuncType.Value)
            {
                return this.Clone();
            }
            throw new CastException($"Cannot convert type {ObjectType} to {type}");
        }

        public virtual FlObject Invoke(SymbolTable symboltable, List<FlObject> args)
        {
            symboltable.NewScope(ScopeType.Function);
            try
            {
                if (_This != null)
                    symboltable.AddSymbol("this", new Symbol(SymbolType.Constant), _This);
                return Body(_This, args);
            }
            finally
            {
                symboltable.DestroyScope();
            }
        }

        public virtual FlObject Invoke(List<FlObject> args)
        {
            // There is no way to get the pointer to "this", so this call is "unbound"
            return Body(null, args);
        }

        public virtual FlFunction Bind(FlObject self)
        {
            return new FlFunction(Name, self, Body);
        }
    }
}
