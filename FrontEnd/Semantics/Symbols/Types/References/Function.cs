// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Semantics.Symbols.Containers;
using Zenit.Semantics.Symbols.Variables;
using Zenit.Semantics.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Zenit.Semantics.Symbols.Types.References
{
    public class Function : Reference
    {
        /// <summary>
        /// Function's parameters types
        /// </summary>
        public List<IVariable> Parameters { get; }

        /// <summary>
        /// Function's return type
        /// </summary>
        public IVariable Return => this.TryGet<IVariable>($"@ret-{this.Name}");


        public Function(string name, IType returnType, IContainer parent)
            : base (name, BuiltinType.Function, parent)
        {
            this.Parameters = new List<IVariable>();

            // Create the @ret symbol and save it into the function's symbol table
            this.Insert($"{BuiltinSymbol.Return.GetName()}-{this.Name}", new Variable($"{BuiltinSymbol.Return.GetName()}-{this.Name}", returnType, Access.Public, Storage.Mutable, this));
        }

        internal void UpdateReturnType(IType lastExpr)
        {
            throw new NotImplementedException();
        }

        public IVariable CreateParameter(string name, IType typeInfo, Storage storage)
        {
            // Create the parameter symbol
            var symbol = new Variable(name, typeInfo, Access.Private, storage, this);
            
            // Insert it into the Function's symbol table
            this.Insert(name, symbol);

            // Keep track of the parameter name (and position)
            this.Parameters.Add(symbol);

            // Return a reference to the created symbol
            return symbol;
        }

        public bool IsCircularReference => this.HasCircularReferenceWith(this.Return);

        protected bool HasCircularReferenceWith(ISymbol type)
        {
            if (type is Function functype)
            {
                if (this.GetHashCode() == functype.GetHashCode())
                    return true;
                return this.HasCircularReferenceWith(functype.Return);
            }

            if (type is Tuple typSymbol)
                return typSymbol.Elements.Select(s => s.GetTypeSymbol()).Any(t => this.HasCircularReferenceWith(t));

            return false;
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj) || !(obj is Function func))
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
            return this.ToSafeString((this, $"self-{this.Name}"));
        }

        public override string ToValueString()
        {
            return this.ToSafeString((this, $"self-{this.Name}"));
        }

        public override string ToSafeString(params (IType type, string safestr)[] safeTypes)
        {
            var parameters = this.Parameters
                            .Select(s => s?.ToValueString() ?? "?")
                            .ToList();

            var str = $"fn {this.Name}({string.Join(", ", parameters)})->";

            if (safeTypes.Any(st => st.type == this.Return.TypeSymbol))
            {
                str += safeTypes.First(st => st.type == this.Return.TypeSymbol).safestr;
            }
            else if (this.Return.TypeSymbol is Tuple ttype)
            {
                var l = safeTypes.ToList();
                l.Add((this, "...<cyclic func ref>"));
                str += ttype.ToSafeString(l.ToArray());
            }
            else if (this.Return.TypeSymbol is Function ftype)
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
