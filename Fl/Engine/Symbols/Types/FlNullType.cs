// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Objects;
using System.Collections.Generic;

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

        private static FlBool OperatorEqualsImpl(List<FlObject> args)
        {
            FlNull self = args[0] as FlNull;
            FlObject arg = args[1];

            if (arg.Type == _Instance)
                return new FlBool(arg.RawValue == null);

            return new FlBool(self.RawValue.Equals(arg.RawValue));
        }

        private static FlNullType Initialize()
        {
            TypeDescriptor.Builder builder = new TypeDescriptor.Builder();

            builder

                // Class Name
                .WithName("null")

                .ExtendsFrom(FlObjectType.Instance)
                
                .WithActivator(o => FlNull.Value)

                .WithStaticMethod(FlType.OperatorEquals, FlNullType.OperatorEqualsImpl)
                ;

            // Build
            return new FlNullType(builder.Build());
        }
    }
}
