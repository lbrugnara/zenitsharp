// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Objects;
using System.Collections.Generic;

namespace Fl.Engine.Symbols.Types
{
    public class FlIntType : FlType
    {
        private static FlIntType _Instance;

        private FlIntType(TypeDescriptor descr)
            : base (descr)
        {
        }

        public static FlType Instance => _Instance == null ? (_Instance = Initialize()) : _Instance;

        private static FlObject OperatorAddImpl(List<FlObject> args)
        {
            FlInt self = args[0] as FlInt;
            FlObject arg = args[1];

            if (arg.Type == _Instance)
                return new FlInt(self.Value + (int)arg.RawValue);

            throw new UnsupportedOperandException($"Operator '+' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlObject OperatorSubImpl(List<FlObject> args)
        {
            FlInt self = args[0] as FlInt;

            if (args.Count == 1)
                return new FlInt(self.Value * -1);

            FlObject arg = args[1];

            if (arg.Type == _Instance)
                return new FlInt(self.Value - (int)arg.RawValue);

            throw new UnsupportedOperandException($"Operator '-' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlObject OperatorMultImpl(List<FlObject> args)
        {
            FlInt self = args[0] as FlInt;
            FlObject arg = args[1];

            if (arg.Type == _Instance)
                return new FlInt(self.Value * (int)arg.RawValue);

            throw new UnsupportedOperandException($"Operator '*' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlObject OperatorDivImpl(List<FlObject> args)
        {
            FlInt self = args[0] as FlInt;
            FlObject arg = args[1];

            if (arg.Type == _Instance)
                return new FlInt(self.Value / (int)arg.RawValue);

            throw new UnsupportedOperandException($"Operator '/' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlObject OperatorCallImpl(List<FlObject> args)
        {
            FlInt self = args[0] as FlInt;
            FlType type = args[1] as FlType;

            if (type == _Instance)
                return self.Clone();

            if (type == FlCharType.Instance)
                return new FlChar((char)self.RawValue);

            if (type == FlFloatType.Instance)
                return new FlFloat((float)self.RawValue);

            if (type == FlDoubleType.Instance)
                return new FlDouble((double)self.RawValue);

            if (type == FlDecimalType.Instance)
                return new FlDecimal((decimal)self.RawValue);

            if (type == FlStringType.Instance)
                return new FlString(self.RawValue.ToString());

            throw new CastException($"Cannot convert type {self.Type} to {type}");
        }

        private static FlObject OperatorPreIncrImpl(List<FlObject> args)
        {
            FlInt self = args[0] as FlInt;
            self.Value += 1;
            return self;
        }

        private static FlObject OperatorPostIncrImpl(List<FlObject> args)
        {
            FlInt self = args[0] as FlInt;
            var res = self.Clone();
            self.Value += 1;
            return res;
        }

        private static FlObject OperatorPreDecrImpl(List<FlObject> args)
        {
            FlInt self = args[0] as FlInt;
            self.Value -= 1;
            return self;
        }

        private static FlObject OperatorPostDecrImpl(List<FlObject> args)
        {
            FlInt self = args[0] as FlInt;
            var res = self.Clone();
            self.Value -= 1;
            return res;
        }

        private static FlObject OperatorAssignImpl(List<FlObject> args)
        {
            FlInt self = args[0] as FlInt;
            FlObject arg = args[1];

            if (arg.Type == _Instance)
            {
                self.Value = (arg as FlInt).Value;
                return new FlInt(self.Value);
            }
            throw new UnsupportedOperandException($"Operator '=' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlObject OperatorAddAndAssignImpl(List<FlObject> args)
        {
            FlInt self = args[0] as FlInt;
            FlObject arg = args[1];

            if (arg.Type == _Instance)
            {
                self.Value += (arg as FlInt).Value;
                return new FlInt(self.Value);
            }
            throw new UnsupportedOperandException($"Operator '+=' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlObject OperatorSubAndAssignImpl(List<FlObject> args)
        {
            FlInt self = args[0] as FlInt;
            FlObject arg = args[1];

            if (arg.Type == _Instance)
            {
                self.Value -= (arg as FlInt).Value;
                return new FlInt(self.Value);
            }
            throw new UnsupportedOperandException($"Operator '-=' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlObject OperatorMultAndAssignImpl(List<FlObject> args)
        {
            FlInt self = args[0] as FlInt;
            FlObject arg = args[1];

            if (arg.Type == _Instance)
            {
                self.Value *= (arg as FlInt).Value;
                return new FlInt(self.Value);
            }
            throw new UnsupportedOperandException($"Operator '*=' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlObject OperatorDivAndAssingImpl(List<FlObject> args)
        {
            FlInt self = args[0] as FlInt;
            FlObject arg = args[1];

            if (arg.Type == _Instance)
            {
                self.Value /= (arg as FlInt).Value;
                return new FlInt(self.Value);
            }
            throw new UnsupportedOperandException($"Operator '/=' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlBool OperatorEqualsImpl(List<FlObject> args)
        {
            FlInt self = args[0] as FlInt;
            FlObject arg = args[1];

            if (arg.Type == _Instance)
                return new FlBool(self.Value == (int)arg.RawValue);

            return new FlBool(self.RawValue.Equals(arg.RawValue));
        }

        private static FlObject OperatorNotImpl(List<FlObject> args)
        {
            FlInt self = args[0] as FlInt;
            return new FlInt(self.Value * -1);
        }

        private static FlBool OperatorGtImpl(List<FlObject> args)
        {
            FlInt self = args[0] as FlInt;
            FlObject arg = args[1];

            if (arg.Type == _Instance)
                return new FlBool(self.Value > (int)arg.RawValue);

            throw new UnsupportedOperandException($"Operator '>' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlBool OperatorGteImpl(List<FlObject> args)
        {
            FlInt self = args[0] as FlInt;
            FlObject arg = args[1];

            if (arg.Type == _Instance)
                return new FlBool(self.Value >= (int)arg.RawValue);

            throw new UnsupportedOperandException($"Operator '>=' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlBool OperatorLtImpl(List<FlObject> args)
        {
            FlInt self = args[0] as FlInt;
            FlObject arg = args[1];

            if (arg.Type == _Instance)
                return new FlBool(self.Value < (int)arg.RawValue);

            throw new UnsupportedOperandException($"Operator '<' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlBool OperatorLteImpl(List<FlObject> args)
        {
            FlInt self = args[0] as FlInt;
            FlObject arg = args[1];

            if (arg.Type == _Instance)
                return new FlBool(self.Value <= (int)arg.RawValue);

            throw new UnsupportedOperandException($"Operator '<=' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlIntType Initialize()
        {
            TypeDescriptor.Builder builder = new TypeDescriptor.Builder();

            builder

                // Class Name
                .WithName("int")

                .ExtendsFrom(FlObjectType.Instance)

                // Activator
                .WithActivator((obj) => new FlInt(obj != null ? int.Parse(obj.ToString()) : 0))

                // Static constructor
                /*.WithStaticConstructor((args) =>
                {
                    
                })*/

                // Constructors
                //...

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
                .WithStaticProperty("MAX", SymbolType.Constant, new FlInt(int.MaxValue))
                .WithStaticProperty("MIN", SymbolType.Constant, new FlInt(int.MinValue))

                // Instance Methods
                .WithMethod("str", (self, args) => new FlString(self.RawValue.ToString()));

            // Build
            return new FlIntType(builder.Build());
        }
    }
}
