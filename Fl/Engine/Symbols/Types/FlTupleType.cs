// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fl.Engine.Symbols.Types
{
    public class FlTupleType : FlType
    {
        private static FlTupleType _Instance;

        private FlTupleType(TypeDescriptor descr)
            : base (descr)
        {
        }

        public static FlType Instance => _Instance == null ? (_Instance = Initialize()) : _Instance;

        private static FlTupleType Initialize()
        {
            TypeDescriptor.Builder builder = new TypeDescriptor.Builder();

            builder

                // Class Name
                .WithName("tuple")

                // Activator
                .WithActivator((obj) => new FlTuple())

                // Static constructor
                // ...

                // Constructors
                .WithConstructor(new FlConstructor((self, args) => {
                    FlTuple t = self as FlTuple;
                    t.Value.AddRange(args);
                }))

                // Static Methods
                // ...

                // Static Properties
                // ...

                // Instance Methods
                .WithMethod("get", (self, args) => (self as FlTuple).Value.ElementAtOrDefault((args[0] as FlInt).Value))
                .WithMethod("count", (self, args) => new FlInt((self as FlTuple).Value.Count))

                // Indexers
                .WithIndexer(new FlIndexer(1, (self, args) =>
                {
                    FlTuple t = self as FlTuple;
                    FlInt index = args[0] as FlInt;
                    return t.Value[index.Value];
                }));

            // Build
            return new FlTupleType(builder.Build());
        }
    }
}
