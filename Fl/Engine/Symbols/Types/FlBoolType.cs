// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Objects;
using System.Collections.Generic;

namespace Fl.Engine.Symbols.Types
{
    public class FlBoolType : FlType
    {
        private static FlBoolType _Instance;

        private FlBoolType(TypeDescriptor descr)
            : base (descr)
        {
        }

        public static FlType Instance => _Instance == null ? (_Instance = Initialize()) : _Instance;

        private static FlObject OperatorCallImpl(List<FlObject> args)
        {
            FlBool self = args[0] as FlBool;
            FlType type = args[1] as FlType;

            if (type == FlBoolType.Instance)
                return self.Clone();

            if (type == FlStringType.Instance)
                return new FlString(self.RawValue.ToString());

            throw new CastException($"Cannot convert type {self.Type} to {type}");
        }
        
        private static FlObject OperatorAssignImpl(List<FlObject> args)
        {
            FlBool self = args[0] as FlBool;
            FlObject arg = args[1];

            if (arg.Type == FlBoolType.Instance)
            {
                self.Value = (arg as FlBool).Value;
                return new FlBool(self.Value);
            }
            throw new UnsupportedOperandException($"Operator '=' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlObject OperatorNotImpl(List<FlObject> args)
        {
            FlBool self = args[0] as FlBool;
            return new FlBool(!self.Value);
        }

        private static FlBoolType Initialize()
        {
            TypeDescriptor.Builder builder = new TypeDescriptor.Builder();

            builder

                // Class Name
                .WithName("bool")

                // Activator
                .WithActivator((obj) => new FlBool(obj != null ? bool.Parse(obj.ToString()) : false))

                // Static constructor
                // ...

                // Static Methods
                .WithStaticMethod(FlType.OperatorCall, OperatorCallImpl)

                .WithStaticMethod(FlType.OperatorAssign, OperatorAssignImpl)

                .WithStaticMethod(FlType.OperatorNot, OperatorNotImpl)

                // Static Properties
                // ...

                // Instance Methods
                .WithMethod("str", (self, args) => new FlString(self.RawValue.ToString()));

            // Build
            return new FlBoolType(builder.Build());
        }
    }
}
