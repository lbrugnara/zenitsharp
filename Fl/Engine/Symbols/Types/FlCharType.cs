// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Objects;
using System.Collections.Generic;

namespace Fl.Engine.Symbols.Types
{
    public class FlCharType : FlType
    {
        private static FlCharType _Instance;

        private FlCharType(TypeDescriptor descr)
            : base (descr)
        {
        }

        public static FlType Instance => _Instance == null ? (_Instance = Initialize()) : _Instance;

        private static FlObject OperatorAddImpl(List<FlObject> args)
        {
            FlChar self = args[0] as FlChar;
            FlObject arg = args[1];

            if (arg.Type == FlIntType.Instance)
                return new FlChar((char)(self.Value + (int)arg.RawValue));

            if (arg.Type == FlStringType.Instance)
                return new FlString(self.RawValue.ToString() + arg.RawValue.ToString());

            throw new UnsupportedOperandException($"Operator '+' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlObject OperatorSubImpl(List<FlObject> args)
        {
            FlChar self = args[0] as FlChar;

            if (args.Count == 1)
                throw new UnsupportedOperandException($"Operator '-' cannot be applied to operand of type '{self.Type}'");

            FlObject arg = args[1];

            if (arg.Type == FlIntType.Instance)
                return new FlInt(self.Value - (int)arg.RawValue);

            throw new UnsupportedOperandException($"Operator '-' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlObject OperatorCallImpl(List<FlObject> args)
        {
            FlChar self = args[0] as FlChar;
            FlType type = args[1] as FlType;

            if (type == FlCharType.Instance)
                return self.Clone();

            if (type == FlIntType.Instance)
                return new FlInt((int)self.RawValue);

            if (type == FlStringType.Instance)
                return new FlString(self.RawValue.ToString());

            throw new CastException($"Cannot convert type {self.Type} to {type}");
        }

        private static FlObject OperatorPreIncrImpl(List<FlObject> args)
        {
            FlChar self = args[0] as FlChar;
            self.Value += (char)1;
            return self;
        }

        private static FlObject OperatorPostIncrImpl(List<FlObject> args)
        {
            FlChar self = args[0] as FlChar;
            var res = self.Clone();
            self.Value += (char)1;
            return res;
        }

        private static FlObject OperatorPreDecrImpl(List<FlObject> args)
        {
            FlChar self = args[0] as FlChar;
            self.Value -= (char)1;
            return self;
        }

        private static FlObject OperatorPostDecrImpl(List<FlObject> args)
        {
            FlChar self = args[0] as FlChar;
            var res = self.Clone();
            self.Value -= (char)1;
            return res;
        }

        private static FlObject OperatorAssignImpl(List<FlObject> args)
        {
            FlChar self = args[0] as FlChar;
            FlObject arg = args[1];

            if (arg.Type == FlCharType.Instance)
            {
                self.Value = (arg as FlChar).Value;
                return new FlChar(self.Value);
            }
            throw new UnsupportedOperandException($"Operator '=' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlObject OperatorAddAndAssignImpl(List<FlObject> args)
        {
            FlChar self = args[0] as FlChar;
            FlObject arg = args[1];

            if (arg.Type == FlCharType.Instance)
            {
                self.Value += (arg as FlChar).Value;
                return new FlChar(self.Value);
            }
            throw new UnsupportedOperandException($"Operator '+=' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlObject OperatorSubAndAssignImpl(List<FlObject> args)
        {
            FlChar self = args[0] as FlChar;
            FlObject arg = args[1];

            if (arg.Type == FlCharType.Instance)
            {
                self.Value -= (arg as FlChar).Value;
                return new FlChar(self.Value);
            }
            throw new UnsupportedOperandException($"Operator '-=' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlCharType Initialize()
        {
            TypeDescriptor.Builder builder = new TypeDescriptor.Builder();

            builder

                // Class Name
                .WithName("char")

                // Activator
                .WithActivator((obj) => new FlChar(obj != null ? char.Parse(obj.ToString()) : '\0'))

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
                
                .WithStaticMethod(FlType.OperatorAssign, OperatorAssignImpl)
                .WithStaticMethod(FlType.OperatorAddAndAssign, OperatorAddAndAssignImpl)
                .WithStaticMethod(FlType.OperatorSubAndAssign, OperatorSubAndAssignImpl)

                // Instance Methods
                .WithMethod("str", (self, args) => new FlString(self.RawValue.ToString()));

            // Build
            return new FlCharType(builder.Build());
        }
    }
}
