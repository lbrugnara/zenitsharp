// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fl.Engine.Symbols.Objects
{
    public class FlFunction: FlObject
    {
        /// <summary>
        /// Represents a function that has no 'this' keyword bound
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public delegate FlObject UnboundFunction(List<FlObject> args);

        /// <summary>
        /// Represents a function that has the self object bound to the 'this' keyword
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public delegate FlObject BoundFunction(FlObject self, List<FlObject> args);

        public class ContractException : Exception
        {
            public ContractException(string msg)
                : base (msg)
            {
            }
        }

        public class Contract
        {
            public FlType SelfType { get; }
            public int NumParams { get; }
            public List<FlType> ParamTypes { get; }

            public Contract(int nparams, List<FlType> paramTypes)
            {
                this.SelfType = FlNullType.Instance;
                this.NumParams = nparams;
                this.ParamTypes = paramTypes ?? new List<FlType>();
            }

            public Contract(FlType selfType, int nparams, List<FlType> paramTypes)
                : this(nparams, paramTypes)
            {
                this.SelfType = selfType;
            }

            public void Ensure(FlObject self, List<FlObject> args)
            {
                if (self.Type != this.SelfType)
                    throw new ContractException($"Function expects bound object to be of type {this.SelfType} but it is of type {self.Type}.");

                if (args.Count != this.NumParams)
                    throw new ContractException($"Function expects {this.NumParams} argument{(this.NumParams == 1 ? "" : "s")} but received {args.Count}.");

                for (int i = 0; i < args.Count; i++)
                    if (args[i].Type != this.ParamTypes[i])
                        throw new ContractException($"Function expects argument {i+1} to be of type {this.ParamTypes[i]} but received {args[i].Type}.");
            }
        }
        

        private string _Name;
        private FlObject _This;
        private BoundFunction _Body;
        private Contract _Contract;

        protected FlFunction() { }

        /// <summary>
        /// Body is an unbound function that receives a list of arguments
        /// and returns an FlObject
        /// </summary>
        /// <param name="name">Function name</param>
        /// <param name="body">Lambda that receives a list of FlObjects and returns an FlObject</param>
        public FlFunction(string name, UnboundFunction body, Contract contract = null)
        {
            _Name = name ?? "anonymous";
            _This = FlNull.Value;
            _Body = (self, args) => body(args);
            _Contract = contract;
        }

        /// <summary>
        /// Body is an bound function that receives a list of arguments along with an object that represents
        /// the this pointer that will be the value referenced by self
        /// and returns an FlObject
        /// </summary>
        /// <param name="name">Function name</param>
        /// <param name="self">Function name</param>
        /// <param name="body">Lambda that receives a list of FlObjects and returns an FlObject</param>
        protected FlFunction(string name, FlObject self, BoundFunction body, Contract contract = null)
        {
            _Name = name ?? "anonymous";
            _This = self;
            _Body = body;
            _Contract = contract;
        }

        public void CopyFrom(FlFunction f)
        {
            _Body = f.Body;
            _This = f.This;
            _Name = f.Name;
            _Contract = f._Contract;
        }

        public virtual FlObject This => _This;

        public virtual BoundFunction Body => _Body;

        public virtual string Name => _Name;

        public override object RawValue => Body;

        public override FlType Type => FlFuncType.Instance;

        public override FlObject Clone()
        {
            return new FlFunction(Name, This, Body);
        }

        public override string ToString()
        {
            return Name;
        }

        public override string ToDebugStr()
        {
            return $"{Name} (func)";
        }

        public static Func<FlFunction> Activator => () => new FlFunction();

        protected virtual void CreateFunctionScope(SymbolTable symboltable)
        {
            symboltable.EnterScope(ScopeType.Function);
        }

        public virtual FlObject Invoke(SymbolTable symboltable, params FlObject[] args) => Invoke(symboltable, args.ToList());

        public virtual FlObject Invoke(SymbolTable symboltable, List<FlObject> args)
        {
            if (_Contract != null)
                _Contract.Ensure(_This, args);

            CreateFunctionScope(symboltable);
            try
            {
                if (this.IsBound())
                    symboltable.AddSymbol("this", new Symbol(SymbolType.Constant), _This);
                return Body(_This, args);
            }
            finally
            {
                symboltable.LeaveScope();
            }
        }

        public bool IsBound() => _This != null && _This != FlNull.Value;

        public virtual FlFunction Bind(FlObject self)
        {
            return new FlFunction(Name, self, Body, _Contract);
        }
    }
}
