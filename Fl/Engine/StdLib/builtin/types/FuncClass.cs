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
            List<FlConstructor> constructors = new List<FlConstructor>();
            Dictionary<string, Symbol> methods = new Dictionary<string, Symbol>();
            Dictionary<string, Symbol> properties = new Dictionary<string, Symbol>();
            Dictionary<string, Symbol> staticMethods = new Dictionary<string, Symbol>();
            Dictionary<string, Symbol> staticProperties = new Dictionary<string, Symbol>();

            Func<FlObject> activator = () => FlNull.Value;

            // Instance Methods
            var str = new Symbol(SymbolType.Constant);
            str.DoBinding("Func", "Str", new FlMethod("Str", (self, args) => new FlString(self.ToString())));
            methods.Add(str.Name, str);

            var invoke = new Symbol(SymbolType.Constant);
            invoke.DoBinding("Func", "Invoke", new FlMethod("Invoke", (self, args) => (self as FlFunction).Invoke(args)));
            methods.Add(invoke.Name, invoke);

            var bind = new Symbol(SymbolType.Constant);
            bind.DoBinding("Func", "Bind", new FlMethod("Bind", (self, args) => (self as FlFunction).Bind(args[0])));
            methods.Add(bind.Name, bind);

            // Build
            return new FlClass(new ClassDescriptor("Func", activator, constructors, methods, properties, null, staticMethods, staticProperties));
        }
    }
}
