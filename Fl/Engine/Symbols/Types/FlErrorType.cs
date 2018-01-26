// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Objects;

namespace Fl.Engine.Symbols.Types
{
    public class FlErrorType : FlType
    {
        private static FlErrorType _Instance;

        private FlErrorType(TypeDescriptor descr)
            : base (descr)
        {
        }

        public static FlType Instance => _Instance == null ? (_Instance = Initialize()) : _Instance;

        private static FlErrorType Initialize()
        {
            TypeDescriptor.Builder builder = new TypeDescriptor.Builder();

            builder

                // Class Name
                .WithName("Error")

                // Activator
                // .WithActivator(() => new FlInstance())

                // Static constructor
                // ...

                // Constructors
                // .WithConstructor(new FlConstructor((self, args) => args.ForEach(a => self.AddAndAssign(a))))

                // Static Methods
                // ...

                // Static Properties
                // ...

                // Instance Methods
                // ...

                // Instance Properties
                .WithProperty("message", SymbolType.Variable, new FlString(null));

                // Indexers
                // ...

            // Build
            return new FlErrorType(builder.Build());
        }
    }
}
