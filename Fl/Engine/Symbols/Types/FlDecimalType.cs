// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Symbols.Types
{
    public class FlDecimalType : FlType
    {
        private static FlDecimalType _Instance;

        private FlDecimalType(TypeDescriptor descr)
            : base (descr)
        {
        }

        public static FlType Instance => _Instance == null ? (_Instance = Initialize()) : _Instance;

        private static FlObject OperatorAddImpl(List<FlObject> args)
        {
            FlDecimal self = args[0] as FlDecimal;
            FlObject arg = args[1];

            if (arg.Type == FlDecimalType.Instance)
                return new FlDecimal(self.Value + (decimal)arg.RawValue);

            throw new UnsupportedOperandException($"Operator '+' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlObject OperatorSubImpl(List<FlObject> args)
        {
            FlDecimal self = args[0] as FlDecimal;

            if (args.Count == 1)
                return new FlDecimal(self.Value * -1.0M);

            FlObject arg = args[1];

            if (arg.Type == FlDecimalType.Instance)
                return new FlDecimal(self.Value - (decimal)arg.RawValue);

            throw new UnsupportedOperandException($"Operator '-' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlObject OperatorMultImpl(List<FlObject> args)
        {
            FlDecimal self = args[0] as FlDecimal;
            FlObject arg = args[1];

            if (arg.Type == FlDecimalType.Instance)
                return new FlDecimal(self.Value * (decimal)arg.RawValue);

            throw new UnsupportedOperandException($"Operator '*' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlObject OperatorDivImpl(List<FlObject> args)
        {
            FlDecimal self = args[0] as FlDecimal;
            FlObject arg = args[1];

            if (arg.Type == FlDecimalType.Instance)
                return new FlDecimal(self.Value / (decimal)arg.RawValue);

            throw new UnsupportedOperandException($"Operator '/' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlObject OperatorCallImpl(List<FlObject> args)
        {
            FlDecimal self = args[0] as FlDecimal;
            FlType type = args[1] as FlType;

            if (type == FlDecimalType.Instance)
                return self.Clone();

            if (type == FlCharType.Instance)
                return new FlChar((char)self.RawValue);

            if (type == FlFloatType.Instance)
                return new FlFloat((float)self.RawValue);

            if (type == FlDoubleType.Instance)
                return new FlDouble((double)self.RawValue);

            if (type == FlIntType.Instance)
                return new FlInt((int)self.RawValue);

            if (type == FlStringType.Instance)
                return new FlString(self.RawValue.ToString());

            throw new CastException($"Cannot convert type {self.Type} to {type}");
        }

        private static FlObject OperatorPreIncrImpl(List<FlObject> args)
        {
            FlDecimal self = args[0] as FlDecimal;
            self.Value += 1.0M;
            return self;
        }

        private static FlObject OperatorPostIncrImpl(List<FlObject> args)
        {
            FlDecimal self = args[0] as FlDecimal;
            var res = self.Clone();
            self.Value += 1.0M;
            return res;
        }

        private static FlObject OperatorPreDecrImpl(List<FlObject> args)
        {
            FlDecimal self = args[0] as FlDecimal;
            self.Value -= 1.0M;
            return self;
        }

        private static FlObject OperatorPostDecrImpl(List<FlObject> args)
        {
            FlDecimal self = args[0] as FlDecimal;
            var res = self.Clone();
            self.Value -= 1.0M;
            return res;
        }

        private static FlObject OperatorAssignImpl(List<FlObject> args)
        {
            FlDecimal self = args[0] as FlDecimal;
            FlObject arg = args[1];

            if (arg.Type == FlDecimalType.Instance)
            {
                self.Value = (arg as FlDecimal).Value;
                return new FlDecimal(self.Value);
            }
            throw new UnsupportedOperandException($"Operator '=' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlObject OperatorAddAndAssignImpl(List<FlObject> args)
        {
            FlDecimal self = args[0] as FlDecimal;
            FlObject arg = args[1];

            if (arg.Type == FlDecimalType.Instance)
            {
                self.Value += (arg as FlDecimal).Value;
                return new FlDecimal(self.Value);
            }
            throw new UnsupportedOperandException($"Operator '+=' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlObject OperatorSubAndAssignImpl(List<FlObject> args)
        {
            FlDecimal self = args[0] as FlDecimal;
            FlObject arg = args[1];

            if (arg.Type == FlDecimalType.Instance)
            {
                self.Value -= (arg as FlDecimal).Value;
                return new FlDecimal(self.Value);
            }
            throw new UnsupportedOperandException($"Operator '-=' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlObject OperatorMultAndAssignImpl(List<FlObject> args)
        {
            FlDecimal self = args[0] as FlDecimal;
            FlObject arg = args[1];

            if (arg.Type == FlDecimalType.Instance)
            {
                self.Value *= (arg as FlDecimal).Value;
                return new FlDecimal(self.Value);
            }
            throw new UnsupportedOperandException($"Operator '*=' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlObject OperatorDivAndAssingImpl(List<FlObject> args)
        {
            FlDecimal self = args[0] as FlDecimal;
            FlObject arg = args[1];

            if (arg.Type == FlDecimalType.Instance)
            {
                self.Value /= (arg as FlDecimal).Value;
                return new FlDecimal(self.Value);
            }
            throw new UnsupportedOperandException($"Operator '/=' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlBool OperatorEqualsImpl(List<FlObject> args)
        {
            FlDecimal self = args[0] as FlDecimal;
            FlObject arg = args[1];

            if (arg.Type == FlIntType.Instance)
                return new FlBool(self.Value == (decimal)arg.RawValue);

            return new FlBool(self.RawValue.Equals(arg.RawValue));
        }

        private static FlObject OperatorNotImpl(List<FlObject> args)
        {
            FlDecimal self = args[0] as FlDecimal;
            return new FlDecimal(self.Value * -1.0M);
        }

        private static FlBool OperatorGtImpl(List<FlObject> args)
        {
            FlDecimal self = args[0] as FlDecimal;
            FlObject arg = args[1];

            if (arg.Type == FlDecimalType.Instance)
                return new FlBool(self.Value > (decimal)arg.RawValue);

            throw new UnsupportedOperandException($"Operator '>' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlBool OperatorGteImpl(List<FlObject> args)
        {
            FlDecimal self = args[0] as FlDecimal;
            FlObject arg = args[1];

            if (arg.Type == FlDecimalType.Instance)
                return new FlBool(self.Value >= (decimal)arg.RawValue);

            throw new UnsupportedOperandException($"Operator '>=' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlBool OperatorLtImpl(List<FlObject> args)
        {
            FlDecimal self = args[0] as FlDecimal;
            FlObject arg = args[1];

            if (arg.Type == FlDecimalType.Instance)
                return new FlBool(self.Value < (decimal)arg.RawValue);

            throw new UnsupportedOperandException($"Operator '<' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlBool OperatorLteImpl(List<FlObject> args)
        {
            FlDecimal self = args[0] as FlDecimal;
            FlObject arg = args[1];

            if (arg.Type == FlDecimalType.Instance)
                return new FlBool(self.Value <= (decimal)arg.RawValue);

            throw new UnsupportedOperandException($"Operator '<=' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlDecimalType Initialize()
        {
            TypeDescriptor.Builder builder = new TypeDescriptor.Builder();

            builder
                
                // Class Name
                .WithName("decimal")

                // Activator
                .WithActivator((obj) => new FlDecimal(obj != null ? decimal.Parse(obj.ToString()) : 0.0m))

                // Static constructor
                // ...

                // Constructors
                // ...

                // Static Methods
                .WithStaticMethod(FlType.OperatorCall, OperatorCallImpl)

                .WithStaticMethod(FlType.OperatorPreIncr, OperatorPreIncrImpl)
                .WithStaticMethod(FlType.OperatorPostIncr, OperatorPostIncrImpl)
                .WithStaticMethod(FlType.OperatorPreDecr, OperatorPreDecrImpl)
                .WithStaticMethod(FlType.OperatorPostDecr, OperatorPostDecrImpl)

                .WithStaticMethod(FlType.OperatorAdd, OperatorAddImpl)
                .WithStaticMethod(FlType.OperatorSub, OperatorSubImpl)
                .WithStaticMethod(FlType.OperatorMult, OperatorMultImpl)
                .WithStaticMethod(FlType.OperatorDiv, OperatorDivImpl)

                .WithStaticMethod(FlType.OperatorAssign, OperatorAssignImpl)
                .WithStaticMethod(FlType.OperatorAddAndAssign, OperatorAddAndAssignImpl)
                .WithStaticMethod(FlType.OperatorSubAndAssign, OperatorSubAndAssignImpl)
                .WithStaticMethod(FlType.OperatorMultAndAssign, OperatorMultAndAssignImpl)
                .WithStaticMethod(FlType.OperatorDivAndAssing, OperatorDivAndAssingImpl)

                .WithStaticMethod(FlType.OperatorEquals, OperatorEqualsImpl)
                .WithStaticMethod(FlType.OperatorNot, OperatorNotImpl)
                .WithStaticMethod(FlType.OperatorGt, OperatorGtImpl)
                .WithStaticMethod(FlType.OperatorGte, OperatorGteImpl)
                .WithStaticMethod(FlType.OperatorLt, OperatorLtImpl)
                .WithStaticMethod(FlType.OperatorLte, OperatorLteImpl)

                // Static Properties
                .WithStaticProperty("MAX", SymbolType.Constant, new FlDecimal(decimal.MaxValue))
                .WithStaticProperty("MIN", SymbolType.Constant, new FlDecimal(decimal.MinValue))
                
                // Instance Methods
                .WithMethod("str", (self, args) => new FlString(self.RawValue.ToString()));

            // Build
            return new FlDecimalType(builder.Build());
        }
    }
}
