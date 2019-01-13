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
        public IBoundSymbol Return => this.TryGet<IBoundSymbol>("@ret");


        public FunctionSymbol(string name, ITypeSymbol returnType, ISymbolContainer parent)
            : base (name, BuiltinType.Function, parent)
        {
            this.Parameters = new List<IBoundSymbol>();

            // Create the @ret symbol and save it into the function's symbol table
            this.Insert(BuiltinSymbol.Return.GetName(), new BoundSymbol(BuiltinSymbol.Return.GetName(), returnType, Access.Public, Storage.Mutable, this));
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

        public override string ToString()
        {
            return this.ToSafeString((this, "self"));
        }

        public override string ToValueString()
        {
            return this.ToSafeString((this, "self"));
        }

        public override string ToSafeString(params (ITypeSymbol type, string safestr)[] safeTypes)
        {
            var parameters = this.Parameters
                            .Select(s => s?.ToValueString() ?? "?")
                            .ToList();

            var str = $"fn {this.Name}({string.Join(", ", parameters)})->";

            if (safeTypes.Any(st => st.type == this.Return.TypeSymbol))
            {
                str += safeTypes.First(st => st.type == this.Return.TypeSymbol).safestr;
            }
            else if (this.Return.TypeSymbol is TupleSymbol ttype)
            {
                var l = safeTypes.ToList();
                l.Add((this, "...<cyclic func ref>"));
                str += ttype.ToSafeString(l.ToArray());
            }
            else if (this.Return.TypeSymbol is FunctionSymbol ftype)
            {
                var l = safeTypes.ToList();
                l.Add((this, "...<cyclic func ref>"));
                str += ftype.ToSafeString(l.ToArray());
            }
            else
            {
                str += this.Return.TypeSymbol?.ToString() ?? "void";
            }

            return str;
        }
    }
}
