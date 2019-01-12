// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Symbols.Values;
using Fl.Semantics.Types;
using System.Collections.Generic;
using System.Linq;

namespace Fl.Semantics.Symbols
{
    public class FunctionSymbol : ComplexSymbol
    {
        /// <summary>
        /// Function's parameters types
        /// </summary>
        public List<IBoundSymbol> Parameters { get; }

        /// <summary>
        /// Function's return type
        /// </summary>
        public IBoundSymbol Return { get; }


        public FunctionSymbol(string name, ISymbolContainer parent)
            : base (name, BuiltinType.Function, parent)
        {
            this.Parameters = new List<IBoundSymbol>();

            // Create the @ret symbol and save it into the function's symbol table
            this.Return = new BoundSymbol("@ret", new AnonymousSymbol(), Access.Public, Storage.Mutable, this);
            this.Insert("@ret", this.Return);
        }        

        public void UpdateReturnType(ITypeSymbol typeInfo)
        {
            // Update the @ret symbol's type
            // this.Return.TypeSymbol = typeInfo ?? throw new System.ArgumentNullException(nameof(typeInfo), "Return type cannot be null");
        }

        public IBoundSymbol CreateParameter(string name, ITypeSymbol typeInfo, Storage storage)
        {
            // Create the parameter symbol
            var symbol = new BoundSymbol(name, typeInfo, Access.Private, storage, this);
            
            // Insert it into the Function's symbol table
            this.Insert(name, symbol);

            // Keep track of the parameter name (and position)
            this.Parameters.Add(symbol);

            // Return a reference to the created symbol
            return symbol;
        }

        public bool IsCircularReference => this.HasCircularReferenceWith(this.Return);

        protected bool HasCircularReferenceWith(IValueSymbol type)
        {
            if (type is FunctionSymbol functype)
            {
                if (this.GetHashCode() == functype.GetHashCode())
                    return true;
                return this.HasCircularReferenceWith(functype.Return);
            }

            if (type is TupleSymbol typSymbol)
                return typSymbol.Types.Any(t => this.HasCircularReferenceWith(t));

            return false;
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj) || !(obj is FunctionSymbol func))
                return false;

            if (!this.Parameters.SequenceEqual(func.Parameters))
                return false;

            if (this.IsCircularReference && func.IsCircularReference && this.GetHashCode() == func.GetHashCode())
                return true;

            return this.Return == null && func.Return == null
                    || this.Return.GetHashCode() == func.GetHashCode()
                    || this.Return == func.Return;
        }

        public override string ToSafeString(List<(ITypeSymbol type, string safestr)> safeTypes)
        {
            var parameters = this.Parameters
                            .Select(s => s?.ToString() ?? "?")
                            .ToList();

            var str = base.ToString() + "(" + string.Join(", ", parameters) + $"):";

            str += " ";
            if (safeTypes.Any(st => st.type == this.Return))
            {
                str += safeTypes.First(st => st.type == this.Return).safestr;
            }
            else if (this.Return is TupleSymbol ttype)
            {
                safeTypes.Add((this, "...<cyclic func ref>"));
                str += ttype.ToSafeString(safeTypes);
            }
            else if (this.Return is FunctionSymbol ftype)
            {
                safeTypes.Add((this, "...<cyclic func ref>"));
                str += ftype.ToSafeString(safeTypes);
            }
            else
            {
                str += this.Return?.ToString() ?? "void";
            }

            return str;
        }
    }
}
