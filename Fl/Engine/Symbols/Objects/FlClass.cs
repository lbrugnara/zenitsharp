// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fl.Engine.Symbols.Objects
{
    public enum MemberType
    {
        Property,
        Method
    }

    public class ClassDescriptor
    {
        public string ClassName { get; }
        public Func<FlObject> Activator { get; }
        private List<FlConstructor> Constructors;
        private Dictionary<string, Symbol> Methods;
        private Dictionary<string, Symbol> Properties;

        public FlFunction StaticConstructor { get; }
        private static Dictionary<string, Symbol> StaticMethods;
        private static Dictionary<string, Symbol> StaticProperties;

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

    public class FlClass : FlObject
    {
        #region Private Fields
        private ClassDescriptor _Descriptor;
        #endregion

        #region Constructor
        public FlClass(ClassDescriptor descriptor)
        {
            _Descriptor = descriptor ?? throw new ArgumentNullException(nameof(descriptor), "Class descriptor cannot be null");
        }
        #endregion

        #region FlObject implementation
        public override ObjectType ObjectType => ClassType.Value;

        public override object RawValue => _Descriptor;

        public override bool IsPrimitive => false;

        public override FlObject Clone()
        {
            return new FlClass(_Descriptor.Clone());
        }

        public override FlObject ConvertTo(ObjectType type)
        {
            if (type == StringType.Value)
            {
                return new FlString(_Descriptor.ClassName);
            }
            throw new CastException($"Cannot convert type {ObjectType} to {type}");
        }

        public override string ToString()
        {
            return $"class {_Descriptor.ClassName}";
        }
        #endregion

        #region Public properties

        public FlFunction StaticConstructor => _Descriptor.StaticConstructor;

        public FlConstructor GetConstructor(int paramsCount) => _Descriptor.GetConstructor(paramsCount);

        public Func<FlObject> Activator => _Descriptor.Activator ?? (() => new FlInstance(new FlClass(_Descriptor.FreshCopy())));

        public Symbol this[MemberType type, string name] => _Descriptor[type, name];

        public override Symbol this[string member] => _Descriptor[member];

        #endregion
    }
}
