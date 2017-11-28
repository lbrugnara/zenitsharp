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
    public static class DoubleClass
    {


        public static FlClass Build()
        {
            ClassDescriptor.Builder builder = new ClassDescriptor.Builder();

            builder
                
                // Class Name
                .WithName("double")

                // Activator
                .WithActivator(() => new FlDouble(0.0))

                // Static constructor
                .WithStaticConstructor((args) => args[0].ConvertTo(DoubleType.Value))

                // Constructors
                //.WithConstructor(new FlConstructor(1, (self, args) => self.Assign(args[0])))

                // Static Methods
                .WithStaticMethod("parse", (args) =>
                {
                    try { return args[0].ConvertTo(DoubleType.Value); } catch { }
                    return FlNull.Value;
                })

                // Static Properties
                .WithStaticProperty("MAX", SymbolType.Constant, new FlDouble(double.MaxValue))
                .WithStaticProperty("MIN", SymbolType.Constant, new FlDouble(double.MinValue))
                
                // Instance Methods
                .WithMethod("str", (self, args) => new FlString(self.RawValue.ToString()));

            // Build
            return new FlClass(builder.Build());
        }
    }
}
