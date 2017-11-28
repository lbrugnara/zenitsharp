// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fl.Engine.Symbols.Objects
{
    public class ClassDescriptor
    {
        public string ClassName { get; private set; }
        public Func<FlObject> Activator { get; private set; }
        private List<FlConstructor> Constructors;
        private Dictionary<string, Symbol> Methods;
        private Dictionary<string, Symbol> Properties;

        public FlFunction StaticConstructor { get; private set; }
        private Dictionary<string, Symbol> StaticMethods;
        private Dictionary<string, Symbol> StaticProperties;

        public ClassDescriptor(string typeName, 
            Func<FlObject> activator,
            List<FlConstructor> constructors, 
            Dictionary<string, Symbol> methods, Dictionary<string, Symbol> properties, 
            FlFunction staticConstructor, 
            Dictionary<string, Symbol> staticMethods, 
            Dictionary<string, Symbol> staticProperties)
        {
            ClassName = typeName ?? throw new ArgumentNullException(nameof(typeName));
            Activator = activator ?? throw new ArgumentNullException(nameof(activator)); ;
            Constructors = constructors ?? new List<FlConstructor>();            
            Methods = methods ?? new Dictionary<string, Symbol>();
            Properties = properties ?? new Dictionary<string, Symbol>();
            StaticConstructor = staticConstructor;
            StaticMethods = staticMethods ?? new Dictionary<string, Symbol>();
            StaticProperties = staticProperties ?? new Dictionary<string, Symbol>();
        }

        private ClassDescriptor()
        {
            Constructors = new List<FlConstructor>();
            Methods = new Dictionary<string, Symbol>();
            Properties = new Dictionary<string, Symbol>();
            StaticMethods = new Dictionary<string, Symbol>();
            StaticProperties = new Dictionary<string, Symbol>();
        }

        public class Builder
        {
            private ClassDescriptor _Class;

            public Builder()
            {
                _Class = new ClassDescriptor();
            }

            public Builder WithName(string className)
            {
                _Class.ClassName = className;
                return this;
            }

            public Builder WithActivator(Func<FlObject> activator)
            {
                _Class.Activator = activator;
                return this;
            }

            public Builder WithConstructor(FlConstructor constructor)
            {
                _Class.Constructors.Add(constructor);
                return this;
            }

            public Builder WithStaticConstructor(Func<List<FlObject>, FlObject> staticConstructor)
            {
                _Class.StaticConstructor = new FlFunction("static_constructor", staticConstructor);
                return this;
            }

            public Builder WithMethod(string methodName, Func<FlObject, List<FlObject>, FlObject> body)
            {
                var symbol = new Symbol(SymbolType.Constant);
                symbol.DoBinding(_Class.ClassName, methodName, new FlMethod(methodName, body));
                _Class.Methods.Add(symbol.Name, symbol);
                return this;
            }

            public Builder WithProperty(string propertyName, SymbolType type, FlObject value)
            {
                var symbol = new Symbol(type);
                symbol.DoBinding(_Class.ClassName, propertyName, value);
                _Class.Properties.Add(symbol.Name, symbol);
                return this;
            }

            public Builder WithStaticMethod(string methodName, Func<List<FlObject>, FlObject> body)
            {
                var symbol = new Symbol(SymbolType.Constant, StorageType.Static);
                symbol.DoBinding(_Class.ClassName, methodName, new FlFunction(methodName, body));
                _Class.StaticMethods.Add(symbol.Name, symbol);
                return this;
            }

            public Builder WithStaticProperty(string propertyName, SymbolType type, FlObject value)
            {
                var symbol = new Symbol(type, StorageType.Static);
                symbol.DoBinding(_Class.ClassName, propertyName, value);
                _Class.StaticProperties.Add(symbol.Name, symbol);
                return this;
            }

            public ClassDescriptor Build()
            {
                var tmp = _Class;
                _Class = null;
                return tmp;
            }
        }

        public FlConstructor GetConstructor(int paramsCount)
        {
            return Constructors.FirstOrDefault(c => c.Name == $"constructor@{paramsCount}");
        }

        public Symbol this[string n]
        {
            get
            {
                if (Properties.ContainsKey(n))
                    return Properties[n];
                else if (Methods.ContainsKey(n))
                    return Methods[n];
                if (StaticProperties.ContainsKey(n))
                    return StaticProperties[n];
                else if (StaticMethods.ContainsKey(n))
                    return StaticMethods[n];
                throw new SymbolException($"There is no member {n} in class '{ClassName}'");
            }
        }

        public Symbol this[MemberType t, string n]
        {
            get
            {
                switch (t)
                {
                    case MemberType.Method:
                        return Methods.ContainsKey(n) ? Methods[n] : throw new SymbolException($"There is no method '{n}' in class '{ClassName}'");
                    case MemberType.Property:
                        return Properties.ContainsKey(n) ? Properties[n] : throw new SymbolException($"There is no property '{n}' in class '{ClassName}'");
                }
                throw new SymbolException($"There is no member of type {t} '{n}' in class '{ClassName}'");
            }
        }

        public override string ToString()
        {
            return ClassName;
        }

        public ClassDescriptor Clone()
        {
            List<FlConstructor> constructors = new List<FlConstructor>();
            Dictionary<string, Symbol> methods = new Dictionary<string, Symbol>();
            Dictionary<string, Symbol> properties = new Dictionary<string, Symbol>();

            Constructors.ForEach(c => constructors.Add(c.Clone() as FlConstructor));
            foreach (var key in Methods.Keys) methods[key] = Methods[key].Clone(true);
            foreach (var key in Properties.Keys) properties[key] = Properties[key].Clone(true);

            return new ClassDescriptor(ClassName, Activator, constructors, methods, properties, StaticConstructor, StaticMethods, StaticProperties);
        }

        public ClassDescriptor FreshCopy()
        {
            List<FlConstructor> constructors = new List<FlConstructor>();
            Dictionary<string, Symbol> methods = new Dictionary<string, Symbol>();
            Dictionary<string, Symbol> properties = new Dictionary<string, Symbol>();

            Constructors.ForEach(c => constructors.Add(c.Clone() as FlConstructor));
            foreach (var key in Methods.Keys) methods[key] = Methods[key].Clone(true);
            foreach (var key in Properties.Keys) properties[key] = Properties[key].Clone(false);

            return new ClassDescriptor(ClassName, Activator, constructors, methods, properties, StaticConstructor, StaticMethods, StaticProperties);
        }
    }
}
