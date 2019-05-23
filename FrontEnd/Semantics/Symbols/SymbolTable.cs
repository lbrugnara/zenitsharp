// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Linq;
using System.Text;
using Zenit.Semantics.Exceptions;
using Zenit.Semantics.Inferrers;
using Zenit.Semantics.Symbols.Containers;
using Zenit.Semantics.Symbols.Types;
using Zenit.Semantics.Symbols.Types.References;
using Zenit.Semantics.Symbols.Types.Specials;
using Zenit.Semantics.Symbols.Types.Specials.Unresolved;
using Zenit.Semantics.Symbols.Variables;
using Zenit.Semantics.Types;

namespace Zenit.Semantics.Symbols
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

        public Function EnterFunctionScope(string name)
        {
            return this.EnterComplexSymbolScope<Function>(name, this.CurrentScope);
        }

        public Object EnterObjectScope(string name)
        {
            return this.EnterComplexSymbolScope<Object>(name, this.CurrentScope);
        }

        public Class EnterClassScope(string name)
        {
            return this.EnterComplexSymbolScope<Class>(name, this.Global);
        }

        public Tuple EnterTupleScope(string name)
        {
            return this.EnterComplexSymbolScope<Tuple>(name, this.CurrentScope);
        }

        private T EnterComplexSymbolScope<T>(string name, IContainer parent)
            where T : Reference
        {
            T scope = parent.TryGet<T>(name);

            if (scope != null)
            {
                this.CurrentScope = scope;
                return scope;
            }

            if (typeof(T) == typeof(Function))
            {
                scope = new Function(name, new None(), parent) as T;
            }
            else if (typeof(T) == typeof(Object))
            {
                scope = new Object(name, parent) as T;
            }
            else if (typeof(T) == typeof(Class))
            {
                scope = new Class(name, parent) as T;
            }
            else if (typeof(T) == typeof(Tuple))
            {
                scope = new Tuple(name, parent) as T;
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

        public Function GetCurrentFunction()
        {
            var scope = this.CurrentScope;
            while (scope != null)
            {
                if (scope is Function fs)
                    return fs;

                scope = scope.Parent;
            }

            throw new ScopeException("Current scope is not a function");
        }

        #region ISymbolTable implementation

        public IVariable AddNewVariableSymbol(string name, IType type, Access access, Storage storage)
        {
            var symbol = new Variable(name, type, access, storage, this.CurrentScope);
            this.CurrentScope.Insert(name, symbol);
            return symbol;
        }        

        public bool HasVariableSymbol(string name) => this.CurrentScope.Contains<IVariable>(name);

        public IVariable GetVariableSymbol(string name) => this.CurrentScope.Get<IVariable>(name);

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

        private IType ResolveSymbolReference(IUnresolvedType uts)
        {
            if (uts is UnresolvedFunctionType ufunc)
            {
                if (!ufunc.Parent.Contains<Function>(ufunc.Name))
                    return null;

                var func = ufunc.Parent.Get<Function>(ufunc.Name);

                if (func.Return.TypeSymbol.BuiltinType == BuiltinType.None)
                    return null;

                return func.Return.TypeSymbol.GetTypeSymbol();
            }
            else if (uts is UnresolvedExpressionType uet)
            {
                IType left = uet.Left;
                IType right = uet.Right;

                if (uet.Left is IUnresolvedType uetl)
                    left = this.ResolveSymbolReference(uetl) ?? uet.Left;

                if (uet.Right is IUnresolvedType uetr)
                    right = this.ResolveSymbolReference(uetr) ?? uet.Right;

                var type = this.TypeInferrer.FindMostGeneralType(left, right);

                if (type == null)
                    return new UnresolvedExpressionType(uet.Parent, left, right);

                return type;
            }
            else if (uts is UnresolvedSymbol us)
            {
                var symbol = us.Parent.TryGet<ISymbol>(us.Name);

                // Symbol not defined, return the same unresolved symbol
                if (symbol == null)
                    return us;
                
                // If this is an IVarible, we will get the IType of it, and if it is an IType, we will get the same reference,
                // so we can safely work with the type here
                var type = symbol.GetTypeSymbol();

                // If the type is not present (odd..), we return null (error)
                if (type == null)
                    return null;

                // If the builtin type is None, keep trying, return the unresolved type
                if (type.BuiltinType == BuiltinType.None)
                    return us;

                // If the parent is not a class or an object, we don't allow forward references to objects types
                // other than functions, in that case return null (error)
                if (!(us.Parent is Object || us.Parent is Class) && !type.IsOfType<Function>())
                    return null;

                // Otherwise, return the type symbol we have resolved
                return type;
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

            var boundSymbols = scope.GetAllOfType<IVariable>().Where(bs => bs.TypeSymbol is IUnresolvedType);

            foreach (var boundSymbol in boundSymbols)
            {
                var resolvedType = this.ResolveSymbolReference(boundSymbol.TypeSymbol as IUnresolvedType);

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

            var boundSymbols = scope.GetAllOfType<IVariable>().Where(bs => bs.TypeSymbol is IUnresolvedType);

            foreach (var boundSymbol in boundSymbols)
            {
                var uts = boundSymbol.TypeSymbol as IUnresolvedType;

                if (uts is UnresolvedFunctionType ufunc)
                {
                    if (!ufunc.Parent.Contains<Function>(ufunc.Name))
                        throw new SymbolException($"Function '{ufunc.Name}' is not defined in scope '{ufunc.Parent.Name}'.");

                    var func = ufunc.Parent.Get<Function>(ufunc.Name);

                    if (func.Return.TypeSymbol is IUnresolvedType)
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
                else if (uts is UnresolvedSymbol us)
                {
                    if (!us.Parent.Contains<Function>(us.Name))
                        throw new SymbolException($"'{us.Name}' is not defined in scope '{us.Parent.Name}'.");

                    var symbol = us.Parent.Get<IType>(us.Name);

                    if (symbol.BuiltinType == BuiltinType.None)
                        throw new SymbolException($"'{us.Name}' is not defined in scope '{us.Parent.Name}'.");

                    if (!symbol.IsOfType<Function>())
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
