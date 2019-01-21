﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Linq;
using System.Text;
using Fl.Semantics.Exceptions;
using Fl.Semantics.Inferrers;
using Fl.Semantics.Symbols.Containers;
using Fl.Semantics.Symbols.Types;
using Fl.Semantics.Symbols.Types.Specials;
using Fl.Semantics.Symbols.Values;
using Fl.Semantics.Types;

namespace Fl.Semantics.Symbols
{
    public class SymbolTable : ISymbolTable
    {
        /// <summary>
        /// Global scope is the first created (last in the stack) scope
        /// </summary>
        private IContainer Global { get; set; }

        /// <summary>
        /// Current scope is the latest added one
        /// </summary>
        public IContainer CurrentScope { get; private set; }

        private TypeInferrer TypeInferrer { get; }

        public SymbolTable(TypeInferrer typeInferrer)
        {
            // Create the @global scope and set it as the current scope
            this.Global = this.CurrentScope = new Block("@global");
            //this.CurrentScope.Insert<Expressions.Package>("myPackage", new Expressions.Package("myPackage", this.CurrentScope));
            this.TypeInferrer = typeInferrer;
        }

        /// <summary>
        /// Check if there's a child scope in the current scope with the provided UID.
        /// If scope does not exist, create a new scope and chain it to the current scope.
        /// Either way, push retrieved/created scope to the stack to make it the current scope.
        /// </summary>
        /// <param name="type">Type of the scope to get/create</param>
        /// <param name="name">ID of the scope to get/create</param>
        public Block EnterBlockScope(string name)
        {
            return this.EnterBlockScope<Block>(name, this.CurrentScope);
        }

        /// <summary>
        /// Check if there's a child scope in the current scope with the provided UID.
        /// If scope does not exist, create a new scope and chain it to the current scope.
        /// Either way, push retrieved/created scope to the stack to make it the current scope.
        /// </summary>
        /// <param name="type">Type of the scope to get/create</param>
        /// <param name="name">ID of the scope to get/create</param>
        public Loop EnterLoopScope(string name)
        {
            return this.EnterBlockScope<Loop>(name, this.CurrentScope);
        }

        private T EnterBlockScope<T>(string name, IContainer parent)
            where T : Block
        {
            T scope = parent.TryGet<T>(name);

            if (scope != null)
            {
                this.CurrentScope = scope;
                return scope;
            }

            if (typeof(T) == typeof(Loop))
                scope = new Loop(name, parent) as T;
            else if (typeof(T) == typeof(Block))
                scope = new Block(name, parent) as T;
            else
                throw new ScopeException($"Unknown scope type {typeof(T).Name}");

            parent.Insert(name, scope);
            this.CurrentScope = scope;
            return scope;
        }

        public FunctionSymbol EnterFunctionScope(string name)
        {
            return this.EnterComplexSymbolScope<FunctionSymbol>(name, this.CurrentScope);
        }

        public ObjectSymbol EnterObjectScope(string name)
        {
            return this.EnterComplexSymbolScope<ObjectSymbol>(name, this.CurrentScope);
        }

        public ClassSymbol EnterClassScope(string name)
        {
            return this.EnterComplexSymbolScope<ClassSymbol>(name, this.Global);
        }

        private T EnterComplexSymbolScope<T>(string name, IContainer parent)
            where T : ComplexSymbol
        {
            T scope = parent.TryGet<T>(name);

            if (scope != null)
            {
                this.CurrentScope = scope;
                return scope;
            }

            if (typeof(T) == typeof(FunctionSymbol))
            {
                scope = new FunctionSymbol(name, new NoneSymbol(), parent) as T;
            }
            else if (typeof(T) == typeof(ObjectSymbol))
            {
                scope = new ObjectSymbol(name, parent) as T;
            }
            else if (typeof(T) == typeof(ClassSymbol))
            {
                scope = new ClassSymbol(name, parent) as T;
            }
            else
            {
                throw new ScopeException($"Unknown scope type {typeof(T).Name}");
            }

            parent.Insert(name, scope);
            this.CurrentScope = scope;

            return scope;
        }

        /// <summary>
        /// Remove the current scope from the stack (go back to the current scope's parent)
        /// </summary>
        public void LeaveScope()
        {
            if (this.CurrentScope.Parent == null)
                throw new ScopeOperationException($"Scope {this.CurrentScope.Name} does not have a parent scope");

            this.CurrentScope = this.CurrentScope.Parent;
        }

        public FunctionSymbol GetCurrentFunction()
        {
            var scope = this.CurrentScope;
            while (scope != null)
            {
                if (scope is FunctionSymbol fs)
                    return fs;

                scope = scope.Parent;
            }

            throw new ScopeException("Current scope is not a function");
        }

        #region ISymbolTable implementation

        public IBoundSymbol BindSymbol(string name, ITypeSymbol type, Access access, Storage storage)
        {
            var symbol = new BoundSymbol(name, type, access, storage, this.CurrentScope);
            this.CurrentScope.Insert(name, symbol);
            return symbol;
        }        

        public bool HasBoundSymbol(string name) => this.CurrentScope.Contains<IBoundSymbol>(name);

        public IBoundSymbol GetBoundSymbol(string name) => this.CurrentScope.Get<IBoundSymbol>(name);

        #endregion

        /// <summary>
        /// Call <see cref="UpdateSymbolReferences(IContainer)"/> using the global scope as the starting point
        /// </summary>
        public void UpdateSymbolReferences()
        {
            this.UpdateSymbolReferences(this.Global);
        }

        /// <summary>
        /// Starting from the global scope, checks if there are unresolved symbols, and in case there are, this method throws an exception.
        /// </summary>
        public void ThrowIfUnresolved()
        {
            this.ThrowIfUnresolved(this.Global);
        }

        /// <summary>
        /// Iterate through all the IBoundSymbols with IUnresolvedTypeSymbols type in order to update the reference, in 
        /// case the referenced symbol has been defined.
        /// </summary>
        /// <param name="scope"></param>
        private void UpdateSymbolReferences(IContainer scope)
        {
            var containers = scope.GetAllOfType<IContainer>();

            foreach (var container in containers)
                this.UpdateSymbolReferences(container);

            var boundSymbols = scope.GetAllOfType<IBoundSymbol>().Where(bs => bs.TypeSymbol is IUnresolvedTypeSymbol);

            foreach (var boundSymbol in boundSymbols)
            {
                var uts = boundSymbol.TypeSymbol as IUnresolvedTypeSymbol;


                if (uts is UnresolvedFunctionSymbol ufunc)
                {
                    if (!ufunc.Parent.Contains<FunctionSymbol>(ufunc.Name))
                        continue;

                    var func = ufunc.Parent.Get<FunctionSymbol>(ufunc.Name);

                    if (func.Return.TypeSymbol.BuiltinType == BuiltinType.None)
                        continue;

                    boundSymbol.ChangeType(func.Return.TypeSymbol.GetTypeSymbol());
                }
                else if (uts is UnresolvedTypeSymbol us)
                {
                    if (!us.Parent.Contains<FunctionSymbol>(us.Name))
                        continue;

                    var symbol = us.Parent.Get<ITypeSymbol>(us.Name);

                    if (symbol.BuiltinType == BuiltinType.None)
                        continue;

                    if (!symbol.IsOfType<FunctionSymbol>())
                        continue;

                    boundSymbol.ChangeType(symbol.GetTypeSymbol());
                }
            }
        }        

        /// <summary>
        /// Iterate all the unresolved symbols in order to determine if they are not defined or if they are 
        /// cyclic references, and in that case assing an anonymous type
        /// </summary>
        /// <param name="scope"></param>
        private void ThrowIfUnresolved(IContainer scope)
        {
            var containers = scope.GetAllOfType<IContainer>();

            foreach (var container in containers)
                this.ThrowIfUnresolved(container);

            var boundSymbols = scope.GetAllOfType<IBoundSymbol>().Where(bs => bs.TypeSymbol is IUnresolvedTypeSymbol);

            foreach (var boundSymbol in boundSymbols)
            {
                var uts = boundSymbol.TypeSymbol as IUnresolvedTypeSymbol;

                if (uts is UnresolvedFunctionSymbol ufunc)
                {
                    if (!ufunc.Parent.Contains<FunctionSymbol>(ufunc.Name))
                        throw new SymbolException($"Function '{ufunc.Name}' is not defined in scope '{ufunc.Parent.Name}'.");

                    var func = ufunc.Parent.Get<FunctionSymbol>(ufunc.Name);

                    if (func.Return.TypeSymbol is IUnresolvedTypeSymbol)
                    {
                        // Circular reference?
                        if (boundSymbol.TypeSymbol == func.Return.TypeSymbol)
                        {
                            this.TypeInferrer.NewAnonymousTypeFor(func.Return);
                        }
                        else
                        {
                            throw new SymbolException($"Function '{ufunc.Name}' is not defined in scope '{ufunc.Parent.Name}'.");
                        }
                    }

                    boundSymbol.ChangeType(func.Return.TypeSymbol.GetTypeSymbol());
                }
                else if (uts is UnresolvedTypeSymbol us)
                {
                    if (!us.Parent.Contains<FunctionSymbol>(us.Name))
                        throw new SymbolException($"'{us.Name}' is not defined in scope '{us.Parent.Name}'.");

                    var symbol = us.Parent.Get<ITypeSymbol>(us.Name);

                    if (symbol.BuiltinType == BuiltinType.None)
                        throw new SymbolException($"'{us.Name}' is not defined in scope '{us.Parent.Name}'.");

                    if (!symbol.IsOfType<FunctionSymbol>())
                        throw new SymbolException($"'{us.Name}' is not defined in scope '{us.Parent.Name}'.");

                    boundSymbol.ChangeType(symbol.GetTypeSymbol());
                }
            }
        }

        public string ToDebugString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("[Symbol Table]");

            sb.AppendLine(this.Global.ToDumpString());

            return sb.ToString();
        }
    }
}
