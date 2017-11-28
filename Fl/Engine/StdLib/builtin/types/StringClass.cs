// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.StdLib.builtin.types
{
    public static class StringClass
    {
        public static FlClass Build()
        {
            ClassDescriptor.Builder builder = new ClassDescriptor.Builder();

            builder

                // Class Name
                .WithName("string")

                // Activator
                .WithActivator(() => new FlString(""))

                // Static constructor
                .WithStaticConstructor((args) => args[0].ConvertTo(StringType.Value))

                // Constructors
                .WithConstructor(new FlConstructor(1, (self, args) => self.Assign(args[0])))

                // Static Methods
                // ...

                // Static Properties
                // ...

                // Instance Methods
                .WithMethod("lower", (self, args) => new FlString(self.RawValue.ToString().ToLower()))
                .WithMethod("upper", (self, args) => new FlString(self.RawValue.ToString().ToUpper()));

            // Build
            return new FlClass(builder.Build());
        }
    }
}
