// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fl.Engine.StdLib.builtin.types
{
    public static class TupleClass
    {
        public static FlClass Build()
        {
            ClassDescriptor.Builder builder = new ClassDescriptor.Builder();

            builder

                // Class Name
                .WithName("Tuple")

                // Activator
                .WithActivator(() => new FlTuple())

                // Static constructor
                // ...

                // Constructors
                .WithConstructor(new FlConstructor((self, args) => args.ForEach(a => self.AddAndAssign(a))))

                // Static Methods
                // ...

                // Static Properties
                // ...

                // Instance Methods
                .WithMethod("get", (self, args) => (self as FlTuple).Value.ElementAtOrDefault((args[0] as FlInteger).Value))

                // Indexers
                .WithIndexer(new FlIndexer(1, (self, args) => self[args[0]]));

            // Build
            return new FlClass(builder.Build());
        }
    }
}
