// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Linq;

namespace Fl.Symbols.Types
{
    public class Function : Struct
    {
        /// <summary>
        /// Contains symbols defined in this function
        /// </summary>
        public Scope Scope { get; private set; }

        /// <summary>
        /// Parameters names that should be defined in the scope
        /// </summary>
        public string[] Parameters { get; private set; }

        public Function(string name, Scope global, string[] parameters)
            : base(name)
        {
            this.Parameters = parameters ?? new string[] { };
            this.Scope = new Scope(ScopeType.Function, name, global);
        }

        /// <summary>
        /// Return type is always the @ret type
        /// </summary>
        public Type Return => this.Scope.GetSymbol("@ret")?.DataType;

        #region ISymbolTable implementation

        /// <inheritdoc/>
        public void AddSymbol(Symbol symbol) => this.Scope.AddSymbol(symbol);

        /// <inheritdoc/>
        public Symbol NewSymbol(string name, Type type) => this.Scope.NewSymbol(name, type);

        /// <inheritdoc/>
        public bool HasSymbol(string name) => this.Scope.HasSymbol(name);

        /// <inheritdoc/>
        public Symbol GetSymbol(string name) => this.Scope.GetSymbol(name);

        #endregion

        public override bool Equals(object obj)
        {
            return base.Equals(obj) 
                && this.Return == (obj as Function).Return
                && this.Parameters.Select(p => this.GetSymbol(p).DataType).SequenceEqual((obj as Function).Parameters.Select(p => this.GetSymbol(p).DataType));
        }

        public static bool operator ==(Function type1, Type type2)
        {
            if (type1 is null)
                return type2 is null;

            return type1.Equals(type2);
        }

        public static bool operator !=(Function type1, Type type2)
        {
            return !(type1 == type2);
        }

        public override string ToString()
        {
            var parameters = this.Parameters.Select(p => this.Scope.GetSymbol(p))
                            .Select(s => s.DataType)
                            .ToList();
            return base.ToString() + "(" + string.Join(", ", parameters) + $"): {this.Return}";
        }

        public override bool IsAssignableFrom(Type type)
        {
            return this.Equals(type);
        }
    }
}
