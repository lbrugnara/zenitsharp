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
    public static class ErrorClass
    {
        public static FlClass Build()
        {
            ClassDescriptor.Builder builder = new ClassDescriptor.Builder();

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
            return new FlClass(builder.Build());
        }
    }
}
