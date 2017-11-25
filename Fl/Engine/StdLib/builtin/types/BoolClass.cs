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
            List<FlConstructor> constructors = new List<FlConstructor>();
            Dictionary<string, Symbol> methods = new Dictionary<string, Symbol>();
            Dictionary<string, Symbol> properties = new Dictionary<string, Symbol>();
            Dictionary<string, Symbol> staticMethods = new Dictionary<string, Symbol>();
            Dictionary<string, Symbol> staticProperties = new Dictionary<string, Symbol>();

            // Activator
            Func<FlObject> activator = () => new FlBool(false);

            // Constructors
            constructors.Add(new FlConstructor(1, (self, args) => (self as FlBool).Value = (args[0] as FlBool).Value));


            // Static Constructor
            FlFunction staticConstructor = new FlFunction("()", (args) => args[0].ConvertTo(BoolType.Value));

            // Static methods
            var parse = new Symbol(SymbolType.Constant, StorageType.Static);
            parse.DoBinding("bool", "Parse", new FlFunction("Parse", (args) =>
            {
                try { return args[0].ConvertTo(BoolType.Value); } catch { }
                return FlNull.Value;
            }));
            staticMethods.Add(parse.Name, parse);
            
            // Static Properties
             
            // Instance Methods
            var str = new Symbol(SymbolType.Constant);
            str.DoBinding("bool", "Str", new FlMethod("Str", (self, args) => new FlString(self.RawValue.ToString())));
            methods.Add(str.Name, str);
            
            // Build
            return new FlClass(new ClassDescriptor("bool", activator, constructors, methods, properties, staticConstructor, staticMethods, staticProperties));
        }
    }
}
