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
    public static class FuncClass
    {
        public static FlClass Build()
        {
            ClassDescriptor.Builder builder = new ClassDescriptor.Builder();

            builder

                // Class Name
                .WithName("func")

                // Activator
                .WithActivator(() => FlFunction.Activator.Invoke() as FlObject)

                // Static constructor
                // ...

                // Constructors
                .WithConstructor(new FlConstructor(1, (self, args) =>
                {
                    var f = (self as FlFunction);
                    var f2 = (args[0] as FlFunction);
                    f.CopyFrom(f2);
                }))


                // Static Methods
                .WithStaticMethod("parse", (args) =>
                {
                    try { return args[0].ConvertTo(BoolType.Value); } catch { }
                    return FlNull.Value;
                })

                // Static Properties
                // ...

                // Instance Methods
                .WithMethod("str", (self, args) => new FlString(self.RawValue.ToString()))
                .WithMethod("invoke", (self, args) => (self as FlFunction).Invoke(SymbolTable.Instance, args))
                .WithMethod("bind", (self, args) => (self as FlFunction).Bind(args[0]))
                .WithMethod("boundTo", (self, args) => (self as FlFunction).This);

            // Build
            return new FlClass(builder.Build());
        }
    }
}
