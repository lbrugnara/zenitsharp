using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.StdLib.builtin.types
{
    public static class IntegerClass
    {
        public static FlClass Build()
        {
            List<FlConstructor> constructors = new List<FlConstructor>();
            Dictionary<string, Symbol> methods = new Dictionary<string, Symbol>();
            Dictionary<string, Symbol> properties = new Dictionary<string, Symbol>();
            Dictionary<string, Symbol> staticMethods = new Dictionary<string, Symbol>();
            Dictionary<string, Symbol> staticProperties = new Dictionary<string, Symbol>();

            // Activator
            Func<FlObject> activator = () => new FlInteger(0);

            // Constructors
            constructors.Add(new FlConstructor(1, (self, args) => new FlOperand(self).Assign(args[0])));


            // Static Constructor
            FlFunction staticConstructor = new FlFunction("()", (args) => args[0].ConvertTo(IntegerType.Value));

            // Static methods
            var parse = new Symbol(SymbolType.Constant, StorageType.Static);
            parse.DoBinding("int", "Parse", new FlFunction("Parse", (args) =>
            {
                try { return args[0].ConvertTo(IntegerType.Value); } catch { }
                return FlNull.Value;
            }));
            staticMethods.Add(parse.Name, parse);
            
            // Static Properties
            var max = new Symbol(SymbolType.Constant, StorageType.Static);
            max.DoBinding("int", "MAX", new FlInteger(int.MaxValue));
            staticProperties.Add(max.Name, max);

            var min = new Symbol(SymbolType.Constant, StorageType.Static);
            min.DoBinding("int", "MIN", new FlInteger(int.MinValue));
            staticProperties.Add(min.Name, min);
            
            // Instance Methods
            var str = new Symbol(SymbolType.Constant);
            str.DoBinding("int", "Str", new FlMethod("Str", (self, args) => new FlString(self.RawValue.ToString())));
            methods.Add(str.Name, str);
            
            // Build
            return new FlClass(new ClassDescriptor("int", activator, constructors, methods, properties, staticConstructor, staticMethods, staticProperties));
        }
    }
}
