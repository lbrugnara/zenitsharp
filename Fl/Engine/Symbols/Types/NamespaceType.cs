// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Objects;

namespace Fl.Engine.Symbols.Types
{
    public class NamespaceType : ObjectType
    {
        private static NamespaceType _Instance;

        private NamespaceType() { }

        public static NamespaceType Value => _Instance != null ? _Instance : (_Instance = new NamespaceType());

        public override string Name => "namespace";

        public override string ClassName => "namespace";

        public override object RawDefaultValue()
        {
            throw new System.NotImplementedException();
        }

        public override FlObject DefaultValue()
        {
            throw new System.NotImplementedException();
        }

        public override FlObject NewValue(object o)
        {
            return new FlNamespace(o.ToString());
        }
    }
}
