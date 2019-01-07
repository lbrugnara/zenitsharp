using System.Collections.Generic;

namespace Fl.Semantics.Types
{
    public class Anonymous : Object
    {
        private string name;
        
        public Anonymous(string name)
            : base("'")
        {
            this.name = name;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj) && this.name == (obj as Anonymous).name;
        }

        public static bool operator ==(Anonymous type1, Object type2)
        {
            if (type1 is null)
                return type2 is null;

            return type1.Equals(type2);
        }

        public static bool operator !=(Anonymous type1, Object type2)
        {
            return !(type1 == type2);
        }

        public override string ToString()
        {
            var assignedName = $"{base.ToString()}{this.name}";

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

        public override bool IsAssignableFrom(Object type)
        {
            return this.Equals(type);
        }
    }
}
