// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Types;
using System;
using System.Linq;

namespace Fl.Engine.Symbols.Objects
{
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

        public string ClassName => _Descriptor.ClassName;

        public FlFunction StaticConstructor => _Descriptor.StaticConstructor;

        public bool HasConstructors => _Descriptor.HasConstructors;

        public FlConstructor GetConstructor(int paramsCount) => _Descriptor.GetConstructor(paramsCount);

        public FlIndexer GetIndexer(int paramsCount) => _Descriptor.GetIndexer(paramsCount);

        public Func<FlObject> Activator => _Descriptor.Activator;

        public Symbol this[MemberType type, string name] => _Descriptor[type, name];

        public override Symbol this[string member] => _Descriptor[member];

        #endregion
    }
}
