// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Semantics.Types
{
    public class ClassMethod : Struct
    {
        public Function Type { get; private set; }
        public AccessModifier AccessModifier { get; }
        public StorageType StorageType { get; }

        public ClassMethod(Function type, AccessModifier accessModifier, StorageType storageType)
            : base("method")
        {
            this.Type = type;
            this.AccessModifier = accessModifier;
            this.StorageType = storageType;
        }

        public static bool operator ==(ClassMethod type1, Type type2)
        {
            if (type1 is null)
                return type2 is null;

            return type1.Equals(type2);
        }

        public static bool operator !=(ClassMethod type1, Type type2)
        {
            return !(type1 == type2);
        }

        public override bool IsAssignableFrom(Type type)
        {
            return this.Equals(type);
        }

        public override string ToString()
        {
            return $"{this.AccessModifier.ToString().ToLower()} {this.Type}";
        }
    }
}
