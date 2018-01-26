// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Objects;
using System.Collections.Generic;

namespace Fl.Engine.Symbols.Types
{
    public class FlStringType : FlType
    {
        private static FlStringType _Instance;

        private FlStringType(TypeDescriptor descr)
            : base (descr)
        {
        }

        public static FlType Instance => _Instance == null ? (_Instance = Initialize()) : _Instance;

        private static FlObject OperatorAddImpl(List<FlObject> args)
        {
            FlString self = args[0] as FlString;
            FlObject arg = args[1];

            if (arg.Type == _Instance || arg.Type == FlCharType.Instance)
                return new FlString(self.Value + arg.RawValue.ToString());

            throw new UnsupportedOperandException($"Operator '+' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlString OperatorCallImpl(List<FlObject> args)
        {
            FlObject arg = args[0];
            return new FlString(arg.RawValue.ToString());
        }

        private static FlObject OperatorAssignImpl(List<FlObject> args)
        {
            FlString self = args[0] as FlString;
            FlObject arg = args[1];

            if (arg.Type == _Instance)
            {
                self.Value = (arg as FlString).Value;
                return new FlString(self.Value);
            }
            throw new UnsupportedOperandException($"Operator '=' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlObject OperatorAddAndAssignImpl(List<FlObject> args)
        {
            FlString self = args[0] as FlString;
            FlObject arg = args[1];

            if (arg.Type == _Instance)
            {
                self.Value += (arg as FlString).Value;
                return new FlString(self.Value);
            }
            throw new UnsupportedOperandException($"Operator '+=' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlBool OperatorEqualsImpl(List<FlObject> args)
        {
            FlString self = args[0] as FlString;
            FlObject arg = args[1];

            return new FlBool(self.RawValue.Equals(arg.RawValue));
        }

        private static FlBool OperatorGtImpl(List<FlObject> args)
        {
            FlString self = args[0] as FlString;
            FlObject arg = args[1];

            if (arg.Type == _Instance)
                return new FlBool(self.Value.CompareTo((arg as FlString).Value) > 0);

            throw new UnsupportedOperandException($"Operator '>' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlBool OperatorGteImpl(List<FlObject> args)
        {
            FlString self = args[0] as FlString;
            FlObject arg = args[1];

            if (arg.Type == _Instance)
                return new FlBool(self.Value.CompareTo((arg as FlString).Value) >= 0);

            throw new UnsupportedOperandException($"Operator '>=' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlBool OperatorLtImpl(List<FlObject> args)
        {
            FlString self = args[0] as FlString;
            FlObject arg = args[1];

            if (arg.Type == _Instance)
                return new FlBool(self.Value.CompareTo((arg as FlString).Value) < 0);

            throw new UnsupportedOperandException($"Operator '<' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlBool OperatorLteImpl(List<FlObject> args)
        {
            FlString self = args[0] as FlString;
            FlObject arg = args[1];

            if (arg.Type == _Instance)
                return new FlBool(self.Value.CompareTo((arg as FlString).Value) <= 0);

            throw new UnsupportedOperandException($"Operator '<=' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlStringType Initialize()
        {
            TypeDescriptor.Builder builder = new TypeDescriptor.Builder();

            builder

                // Class Name
                .WithName("string")

                // Activator
                .WithActivator((obj) => new FlString(obj?.ToString() ?? ""))

                // Static constructor
                //.WithStaticConstructor((args) => args[0].ConvertTo(StringType.Value))

                // Constructors
                .WithConstructor(new FlConstructor(1, (self, args) => (self as FlString).Value = (args[0] as FlString).Value))

                // Static Methods
                .WithStaticMethod(FlType.OperatorCall, OperatorCallImpl)

                .WithStaticMethod(FlType.OperatorAdd, OperatorAddImpl)
                
                .WithStaticMethod(FlType.OperatorAssign, OperatorAssignImpl)
                .WithStaticMethod(FlType.OperatorAddAndAssign, OperatorAddAndAssignImpl)
                
                .WithStaticMethod(FlType.OperatorEquals, OperatorEqualsImpl)
                .WithStaticMethod(FlType.OperatorGt, OperatorGtImpl)
                .WithStaticMethod(FlType.OperatorGte, OperatorGteImpl)
                .WithStaticMethod(FlType.OperatorLt, OperatorLtImpl)
                .WithStaticMethod(FlType.OperatorLte, OperatorLteImpl)

                // Static Properties
                // ...

                // Instance Methods
                .WithMethod("lower", (self, args) => new FlString(self.RawValue.ToString().ToLower()))
                .WithMethod("upper", (self, args) => new FlString(self.RawValue.ToString().ToUpper()));

            // Build
            return new FlStringType(builder.Build());
        }
    }
}
