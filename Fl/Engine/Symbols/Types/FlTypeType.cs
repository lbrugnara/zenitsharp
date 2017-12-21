// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Objects;

namespace Fl.Engine.Symbols.Types
{
    public class FlTypeType : FlType
    {
        private static FlTypeType _Instance;

        private FlTypeType(TypeDescriptor descr)
            : base(descr)
        {
        }

        public static FlType Instance => _Instance == null ? (_Instance = Initialize()) : _Instance;

        private static FlTypeType Initialize()
        {
            TypeDescriptor.Builder builder = new TypeDescriptor.Builder();

            builder

                // Class Name
                .WithName("type")

                .ExtendsFrom(FlObjectType.Instance);

            // Build
            return new FlTypeType(builder.Build());
        }
    }
}
