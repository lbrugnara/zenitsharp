// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics;
using System.Collections.Generic;

namespace Fl.Semantics.Types
{
    public class Object
    {
        public string Name { get; private set; }
        public virtual BuiltinType BuiltinType { get; }
        public Dictionary<string, Object> Properties { get; }
        public Dictionary<string, Function> Functions { get; }

        public Object(BuiltinType type)
            : this(type, type.GetName())
        {
            this.BuiltinType = type;
        }

        public Object(BuiltinType type, string name)
        {
            this.Name = name;
            this.BuiltinType = type;
            this.Properties = new Dictionary<string, Object>();
            this.Functions = new Dictionary<string, Function>();
        }

        public override bool Equals(object obj)
        {
            var objectType = obj as Object;

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

        public static bool operator ==(Object type1, Object type2)
        {
            if (type1 is null)
                return type2 is null;

            return type1.Equals(type2);
        }

        public static bool operator !=(Object type1, Object type2)
        {
            return !(type1 == type2);
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public virtual string ToSafeString(List<(Object type, string safestr)> safeTypes) => this.ToString();

        public override string ToString()
        {
            var assignedName = $"{this.Name}";

            if (this.Properties.Count > 0 || this.Functions.Count > 0)
                assignedName += " {";

            var members = new List<string>();

            foreach (var kvp in this.Properties)
                members.Add($"{kvp.Key}: {kvp.Value}");

            foreach (var kvp in this.Functions)
                members.Add($"{kvp.Key}: {kvp.Value}");

            assignedName += string.Join(", ", members);

            if (this.Properties.Count > 0 || this.Functions.Count > 0)
                assignedName += "} ";

            return assignedName;
        }

        public virtual bool IsAssignableFrom(Object type)
        {
            return this == type;
        }
    }
}
