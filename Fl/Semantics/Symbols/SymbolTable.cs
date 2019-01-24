// Copyright (c) Leonardo Brugnara
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

        /// <summary>
        /// The type inferrer keeps track of unresolved symbols
        /// </summary>
        private TypeInferrer TypeInferrer { get; }

        public SymbolTable(TypeInferrer typeInferrer)
        {
            // Create the @global scope and set it as the current scope
            this.Global = this.CurrentScope = new Block("@global");
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

        private ITypeSymbol ResolveSymbolRference(IUnresolvedTypeSymbol uts)
        {
            if (uts is UnresolvedFunctionSymbol ufunc)
            {
                if (!ufunc.Parent.Contains<FunctionSymbol>(ufunc.Name))
                    return null;

                var func = ufunc.Parent.Get<FunctionSymbol>(ufunc.Name);

                if (func.Return.TypeSymbol.BuiltinType == BuiltinType.None)
                    return null;

                return func.Return.TypeSymbol.GetTypeSymbol();
            }
            else if (uts is UnresolvedExpressionType uet)
            {
                ITypeSymbol left = uet.Left;
                ITypeSymbol right = uet.Right;

                if (uet.Left is IUnresolvedTypeSymbol uetl)
                    left = this.ResolveSymbolRference(uetl) ?? uet.Left;

                if (uet.Right is IUnresolvedTypeSymbol uetr)
                    right = this.ResolveSymbolRference(uetr) ?? uet.Right;

                var type = this.TypeInferrer.FindMostGeneralType(left, right);

                if (type == null)
                    return new UnresolvedExpressionType(uet.Parent, left, right);

                return type;
            }
            else if (uts is UnresolvedTupleType utt)
            {
                ITypeSymbol[] types = new ITypeSymbol[utt.Types.Count];

                for (var i=0; i < utt.Types.Count; i++)
                {
                    var ttype = utt.Types[i];
                    types[i] = ttype is IUnresolvedTypeSymbol uetl ? this.ResolveSymbolRference(uetl) ?? ttype : ttype;
                }

                for (var i = 0; i < utt.Types.Count; i++)
                {
                    utt.Types[i] = types[i];
                }

                return utt.Types.Any(t => t is IUnresolvedTypeSymbol) ? utt : (ITypeSymbol)new TupleSymbol(utt.Parent, utt.Types);
            }
            else if (uts is UnresolvedTypeSymbol us)
            {
                if (!us.Parent.Contains<FunctionSymbol>(us.Name))
                    return null;

                var symbol = us.Parent.Get<ITypeSymbol>(us.Name);

                if (symbol.BuiltinType == BuiltinType.None)
                    return null;

                if (!symbol.IsOfType<FunctionSymbol>())
                    return null;

                return symbol.GetTypeSymbol();
            }
            return null;
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
                var resolvedType = this.ResolveSymbolRference(boundSymbol.TypeSymbol as IUnresolvedTypeSymbol);

                if (resolvedType != null)
                    boundSymbol.ChangeType(resolvedType);
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
