// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Types;
using Fl.Symbols;
using System;
using System.Collections.Generic;

namespace Fl.Engine.Symbols.Objects
{
    public class FlType : FlObject
    {
        #region Private Fields
        private TypeDescriptor _Descriptor;

        public const string OperatorCall             = "@operator ()";
        public const string OperatorAssign           = "@operator =";
        public const string OperatorPreIncr          = "@operator ++_";
        public const string OperatorPostIncr         = "@operator _++";
        public const string OperatorPreDecr          = "@operator --_";
        public const string OperatorPostDecr         = "@operator _--";
        public const string OperatorAdd              = "@operator +";
        public const string OperatorSub              = "@operator -";
        public const string OperatorMult             = "@operator *";
        public const string OperatorDiv              = "@operator /";
        public const string OperatorAddAndAssign     = "@operator +=";
        public const string OperatorSubAndAssign     = "@operator -=";
        public const string OperatorMultAndAssign    = "@operator *=";
        public const string OperatorDivAndAssing     = "@operator /=";
        public const string OperatorEquals           = "@operator ==";
        public const string OperatorNot              = "@operator !";
        public const string OperatorGt               = "@operator >";
        public const string OperatorGte              = "@operator >=";
        public const string OperatorLt               = "@operator <";
        public const string OperatorLte              = "@operator <=";
        #endregion

        #region Constructor
        public FlType(TypeDescriptor descriptor)
        {
            _Descriptor = descriptor ?? throw new ArgumentNullException(nameof(descriptor), "Type descriptor cannot be null");
        }
        #endregion

        #region FlObject implementation
        public override FlType Type => FlTypeType.Instance;

        public override object RawValue => _Descriptor;

        public override FlObject Clone()
        {
            return new FlType(_Descriptor.Clone());
        }
        
        public override string ToString()
        {
            return $"{_Descriptor.Name}";
        }

        public override string ToDebugStr()
        {
            return $"class {_Descriptor.Name}";
        }
        #endregion

        #region Public properties

        public string Name => _Descriptor.Name;

        public FlFunction StaticConstructor => _Descriptor.StaticConstructor;

        public bool HasConstructors => _Descriptor.HasConstructors;

        public FlConstructor GetConstructor(int paramsCount) => _Descriptor.GetConstructor(paramsCount);

        public FlIndexer GetIndexer(int paramsCount) => _Descriptor.GetIndexer(paramsCount);

        public FlStaticMethod GetStaticMethod(string name) => _Descriptor.GetStaticMethod(name);

        public Func<object, FlObject> Activator => _Descriptor.Activator;

        public Symbol this[string member] => _Descriptor[member];

        public FlObject InvokeConstructor(List<FlObject> arguments = null)
        {
            arguments = arguments ?? new List<FlObject>();
            var newInstance = this.Activator.Invoke(null);
            var target = this.GetConstructor(arguments.Count);
            if (!HasConstructors && arguments.Count == 0)
                return newInstance;
            if (target == null)
                throw new SymbolException($"{this} does not contain a constructor that accepts {arguments.Count} {(arguments.Count == 1 ? "argument" : "arguments")}");
            return (target as FlConstructor).Bind(newInstance).Invoke(SymbolTable.Instance, arguments);
        }

        #endregion
    }
}
