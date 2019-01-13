// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Symbols.Values;
using Fl.Semantics.Types;
using System.Collections.Generic;

namespace Fl.Semantics.Symbols
{
    public abstract class TypeSymbol : ITypeSymbol
    {
        /// <summary>
        /// Type symbol
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Built-in type information
        /// </summary>
        public BuiltinType BuiltinType { get; }

        /// <summary>
        /// Symbol's parent
        /// </summary>
        public ISymbolContainer Parent { get; }

        /// <summary>
        /// Type's properties
        /// </summary>
        public Dictionary<string, IValueSymbol> Properties { get; }

        /// <summary>
        /// Type's functions
        /// </summary>
        public Dictionary<string, IValueSymbol> Functions { get; }

        public TypeSymbol(BuiltinType type, ISymbolContainer parent)
        {
            this.Name = type.ToString();
            this.BuiltinType = type;
            this.Parent = parent;
            this.Properties = new Dictionary<string, IValueSymbol>();
            this.Functions = new Dictionary<string, IValueSymbol>();
        }

        protected TypeSymbol(string name, BuiltinType type, ISymbolContainer parent)
            : this (type, parent)
        {
            this.Name = name;
        }

        public override bool Equals(object obj)
        {
            var objectType = obj as TypeSymbol;

            if (obj == null || objectType == null)
                return false;

            // Named types
            if (objectType.BuiltinType.IsNamedType())
                return this.Name == objectType.Name;

            // If both are anonymous, we can't make sure they are equals
            if (this.BuiltinType == BuiltinType.Anonymous && objectType.BuiltinType == BuiltinType.Anonymous)
                return false;

            // If just one of them is anonymous, they are not equals
            if (!this.BuiltinType.IsStructuralType() || !objectType.BuiltinType.IsStructuralType())
                return false;

            // Structural type
            if (this.Properties.Count != objectType.Properties.Count || this.Functions.Count != objectType.Functions.Count)
                return false;

            foreach (var p in this.Properties.Values)
                if (!objectType.Properties.ContainsValue(p))
                    return false;

            foreach (var m in this.Functions.Values)
                if (!objectType.Functions.ContainsValue(m))
                    return false;

            return true;
        }

        public static bool operator ==(TypeSymbol type1, ITypeSymbol type2)
        {
            if (type1 is null)
                return type2 is null;

            return type1.Equals(type2);
        }

        public static bool operator !=(TypeSymbol type1, ITypeSymbol type2)
        {
            return !(type1 == type2);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /*public override string ToString()
        {
            var assignedName = $"{this.BuiltinType.GetName()}";

            if (this.Properties.Count > 0 || this.Functions.Count > 0)
                assignedName += " {";

            var members = new List<string>();

            foreach (var kvp in this.Properties)
                members.Add($"{kvp.Key}: {kvp.Value.ToDebugString()}");

            foreach (var kvp in this.Functions)
                members.Add($"{kvp.Key}: {kvp.Value.ToDebugString()}");

            assignedName += string.Join(", ", members);

            if (this.Properties.Count > 0 || this.Functions.Count > 0)
                assignedName += "} ";

            return assignedName;
        }*/

        public abstract string ToValueString();
    }
}
