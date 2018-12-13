// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Types;

namespace Fl.Semantics.Symbols
{
    public class TypeInfo
    {
        public Object Type { get; set; }

        public TypeInfo(Object type)
        {
            this.Type = type;
        }

        public bool IsAnonymousType => this.Type is Anonymous;

        public override string ToString()
        {
            return this.Type.ToString();
        }
    }
}
