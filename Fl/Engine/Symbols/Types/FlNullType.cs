// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Objects;

namespace Fl.Engine.Symbols.Types
{
    public class FlNullType : FlType
    {
        private static FlNullType _Instance;

        private FlNullType(TypeDescriptor descr)
            : base(descr)
        {
        }

        public static FlType Instance => _Instance == null ? (_Instance = Initialize()) : _Instance;

        private static FlNullType Initialize()
        {
            TypeDescriptor.Builder builder = new TypeDescriptor.Builder();

            builder

                // Class Name
                .WithName("null")

                .ExtendsFrom(FlObjectType.Instance)
                
                .WithActivator(o => FlNull.Value)
                ;

            // Build
            return new FlNullType(builder.Build());
        }
    }
}
