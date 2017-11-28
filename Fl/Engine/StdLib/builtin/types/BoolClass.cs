﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.StdLib.builtin.types
{
    public static class BoolClass
    {
        public static FlClass Build()
        {
            ClassDescriptor.Builder builder = new ClassDescriptor.Builder();

            builder

                // Class Name
                .WithName("bool")

                // Activator
                .WithActivator(() => new FlBool(false))

                // Static constructor
                .WithStaticConstructor((args) => args[0].ConvertTo(BoolType.Value))

                // Constructors
                // ...

                // Static Methods
                .WithStaticMethod("parse", (args) =>
                {
                    try { return args[0].ConvertTo(BoolType.Value); } catch { }
                    return FlNull.Value;
                })

                // Static Properties
                // ...

                // Instance Methods
                .WithMethod("str", (self, args) => new FlString(self.RawValue.ToString()));

            // Build
            return new FlClass(builder.Build());
        }
    }
}
