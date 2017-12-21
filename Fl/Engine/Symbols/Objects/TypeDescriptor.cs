// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fl.Engine.Symbols.Objects
{
    public class TypeDescriptor
    {
        public string Name { get; private set; }
        public Func<object, FlObject> Activator { get; private set; }
        private List<FlConstructor> Constructors;
        private Dictionary<string, Symbol> Methods;
        private Dictionary<string, Symbol> Properties;
        private List<FlIndexer> Indexers;

        public FlFunction StaticConstructor { get; private set; }
        private Dictionary<string, Symbol> StaticMethods;
        private Dictionary<string, Symbol> StaticProperties;

        protected TypeDescriptor Base { get; private set; }

        private TypeDescriptor FreshCopy { get; set; }

        public TypeDescriptor(string typeName,
            TypeDescriptor baseType,
            Func<object, FlObject> activator,
            List<FlConstructor> constructors,
            Dictionary<string, Symbol> methods,
            Dictionary<string, Symbol> properties,
            List<FlIndexer> indexers,
            FlFunction staticConstructor,
            Dictionary<string, Symbol> staticMethods,
            Dictionary<string, Symbol> staticProperties)
            : this(typeName, activator, constructors, methods, properties, indexers, staticConstructor, staticMethods, staticProperties)
        {
            Base = baseType;
        }

        public TypeDescriptor(string typeName, 
            Func<object, FlObject> activator,
            List<FlConstructor> constructors, 
            Dictionary<string, Symbol> methods, 
            Dictionary<string, Symbol> properties,
            List<FlIndexer> indexers,
            FlFunction staticConstructor, 
            Dictionary<string, Symbol> staticMethods, 
            Dictionary<string, Symbol> staticProperties)
        {
            Name = typeName ?? throw new ArgumentNullException(nameof(typeName));
            Activator = activator ?? ((obj) => new FlInstance(new FlType(this.GetFreshCopy())));
            Constructors = constructors ?? new List<FlConstructor>();            
            Methods = methods ?? new Dictionary<string, Symbol>();
            Properties = properties ?? new Dictionary<string, Symbol>();
            Indexers = indexers ?? new List<FlIndexer>();
            StaticConstructor = staticConstructor;
            StaticMethods = staticMethods ?? new Dictionary<string, Symbol>();
            StaticProperties = staticProperties ?? new Dictionary<string, Symbol>();
        }

        private TypeDescriptor()
        {
            Constructors = new List<FlConstructor>();
            Activator = (obj) => new FlInstance(new FlType(this.GetFreshCopy()));
            Methods = new Dictionary<string, Symbol>();
            Properties = new Dictionary<string, Symbol>();
            Indexers = new List<FlIndexer>();
            StaticMethods = new Dictionary<string, Symbol>();
            StaticProperties = new Dictionary<string, Symbol>();
        }

        public bool HasConstructors => Constructors.Any() || (Base != null && Base.HasConstructors);

        public FlConstructor GetConstructor(int paramsCount)
        {
            var ctor = Constructors.FirstOrDefault(c => c.Name == $"constructor@{paramsCount}") ?? Constructors.FirstOrDefault(c => c.Name == $"constructor@params");

            if (ctor != null)
                return ctor;

            return Base != null ? Base.GetConstructor(paramsCount) : null;
        }

        public FlIndexer GetIndexer(int paramsCount)
        {
            var indexer = Indexers.FirstOrDefault(c => c.Name == $"indexer@{paramsCount}");

            if (indexer != null)
                return indexer;

            return Base != null ? Base.GetIndexer(paramsCount) : null;
        }

        public FlFunction GetStaticMethod(string name)
        {
            return StaticMethods.ContainsKey(name) ? StaticMethods[name].Binding as FlFunction : (Base != null ? Base.GetStaticMethod(name) : null);
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

                if (Base != null)
                    return Base[n];

                throw new SymbolException($"There is no member {n} in type '{Name}'");
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public TypeDescriptor Clone()
        {
            List<FlConstructor> constructors = new List<FlConstructor>();
            Dictionary<string, Symbol> methods = new Dictionary<string, Symbol>();
            Dictionary<string, Symbol> properties = new Dictionary<string, Symbol>();
            List<FlIndexer> indexers = new List<FlIndexer>();

            Constructors.ForEach(c => constructors.Add(c.Clone() as FlConstructor));
            foreach (var key in Methods.Keys) methods[key] = Methods[key].Clone(true);
            foreach (var key in Properties.Keys) properties[key] = Properties[key].Clone(true);
            Indexers.ForEach(i => indexers.Add(i.Clone() as FlIndexer));

            return new TypeDescriptor(Name, Base?.Clone(), Activator, constructors, methods, properties, indexers, StaticConstructor, StaticMethods, StaticProperties);
        }

        public TypeDescriptor GetFreshCopy()
        {
            return this.FreshCopy.Clone();
        }

        public class Builder
        {
            private TypeDescriptor _TypeDescriptor;

            public Builder()
            {
                _TypeDescriptor = new TypeDescriptor();
            }

            public Builder WithName(string name)
            {
                _TypeDescriptor.Name = name;
                return this;
            }

            public Builder ExtendsFrom(TypeDescriptor baseType)
            {
                _TypeDescriptor.Base = baseType;
                return this;
            }

            public Builder ExtendsFrom(FlType baseType)
            {
                _TypeDescriptor.Base = baseType.RawValue as TypeDescriptor;
                return this;
            }

            public Builder WithActivator(Func<object, FlObject> activator)
            {
                _TypeDescriptor.Activator = activator;
                return this;
            }

            public Builder WithConstructor(FlConstructor constructor)
            {
                _TypeDescriptor.Constructors.Add(constructor);
                return this;
            }

            public Builder WithStaticConstructor(FlFunction.UnboundFunction staticConstructor)
            {
                _TypeDescriptor.StaticConstructor = new FlFunction("static_constructor", staticConstructor);
                return this;
            }

            public Builder WithMethod(string methodName, FlFunction.BoundFunction body, FlFunction.Contract contract = null)
            {
                var symbol = new Symbol(SymbolType.Constant);
                symbol.DoBinding(_TypeDescriptor.Name, methodName, new FlMethod(methodName, body, contract));
                _TypeDescriptor.Methods.Add(symbol.Name, symbol);
                return this;
            }

            public Builder WithProperty(string propertyName, SymbolType type, FlObject value)
            {
                var symbol = new Symbol(type);
                symbol.DoBinding(_TypeDescriptor.Name, propertyName, value);
                _TypeDescriptor.Properties.Add(symbol.Name, symbol);
                return this;
            }

            public Builder WithIndexer(FlIndexer indexer)
            {
                _TypeDescriptor.Indexers.Add(indexer);
                return this;
            }

            public Builder WithStaticMethod(string methodName, FlFunction.UnboundFunction body)
            {
                var symbol = new Symbol(SymbolType.Constant, StorageType.Static);
                symbol.DoBinding(_TypeDescriptor.Name, methodName, new FlFunction(methodName, body));
                _TypeDescriptor.StaticMethods.Add(symbol.Name, symbol);
                return this;
            }

            public Builder WithStaticProperty(string propertyName, SymbolType type, FlObject value)
            {
                var symbol = new Symbol(type, StorageType.Static);
                symbol.DoBinding(_TypeDescriptor.Name, propertyName, value);
                _TypeDescriptor.StaticProperties.Add(symbol.Name, symbol);
                return this;
            }

            public TypeDescriptor Build()
            {
                _TypeDescriptor.FreshCopy = _TypeDescriptor.Clone();
                var tmp = _TypeDescriptor;
                _TypeDescriptor = null;
                return tmp;
            }
        }
    }
}
