// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Types;

namespace Fl.Semantics.Types
{
    public class TypeInfo
    {
        public Object Type { get; private set; }
        private Object OriginalType { get; }

        public TypeInfo(Object type)
        {
            this.Type = type;
            this.OriginalType = type;
        }

        public bool IsAnonymousType => this.Type.BuiltinType == BuiltinType.Anonymous;

        public bool IsPrimitiveType => this.Type.BuiltinType.IsPrimitive();

        public bool IsStructuralType => this.Type.BuiltinType.IsStructuralType();

        public void ChangeType(Object type)
        {
            this.Type = type;
        }

        public override bool Equals(object obj)
        {
            var type = obj as TypeInfo;

            return type != null && this.Type == type.Type;
        }

        public override string ToString()
        {
            return this.Type.ToString();
        }
    }
}
