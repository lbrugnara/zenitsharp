// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;
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
        public List<string> Parameters { get; private set; }

        public Function(Scope global)
            : base("func")
        {
            this.Parameters = new List<string>();
            this.Scope = new Scope(ScopeType.Function, global.Uid, global);
        }

        /// <summary>
        /// Return type is always the @ret type
        /// </summary>
        public Type Return => this.Scope.GetSymbol("@ret")?.Type;

        public Symbol DefineParameter(string name, Type type)
        {
            if (name == null)
                name = $"#p{this.Parameters.Count}";
            this.Parameters.Add(name);
            return this.NewSymbol(name, type);
        }

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
                && this.Parameters.Select(p => this.GetSymbol(p).Type).SequenceEqual((obj as Function).Parameters.Select(p => this.GetSymbol(p).Type));
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
                            .Select(s => s.Type?.ToString() ?? "?")
                            .ToList();
            return base.ToString() + "(" + string.Join(", ", parameters) + $"): {(this.Return?.ToString() ?? "?")}";
        }

        public override bool IsAssignableFrom(Type type)
        {
            return this.Equals(type);
        }
    }
}
