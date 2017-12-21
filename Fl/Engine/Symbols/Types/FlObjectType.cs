// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Objects;

namespace Fl.Engine.Symbols.Types
{
    public class FlObjectType : FlType
    {
        private static FlObjectType _Instance;

        private FlObjectType(TypeDescriptor descr)
            : base (descr)
        {
        }

        public static FlType Instance => _Instance == null ? (_Instance = Initialize()) : _Instance;

        private static FlObjectType Initialize()
        {
            TypeDescriptor.Builder builder = new TypeDescriptor.Builder();

            builder

                // Class Name
                .WithName("object")

                .WithActivator(o => FlNull.Value)

                // Instance Methods
                .WithMethod("equals", (self, args) => new FlBool(self.RawValue.Equals(args[0])))
                .WithMethod("hash", (self, args) => new FlInt(self.RawValue.GetHashCode()))
                .WithMethod("str", (self, args) => new FlString(self.RawValue.ToString()))
                
                .WithMethod("@this_getter", (self, args) => throw new UnsupportedOperandException($"Cannot apply indexing to object of type '{self.Type}'"))
                .WithMethod("@this_setter", (self, args) => throw new UnsupportedOperandException($"Cannot apply indexing to object of type '{self.Type}'"))

                .WithStaticMethod(FlType.OperatorCall, (args) => throw new UnsupportedOperandException($"Cannot convert {args[1].Type} to {args[0].Type}"))
                
                .WithStaticMethod(FlType.OperatorPreIncr, (args) => throw new UnsupportedOperandException($"Operator '++' cannot be applied to operand of type '{args[0].Type}'"))
                .WithStaticMethod(FlType.OperatorPostIncr, (args) => throw new UnsupportedOperandException($"Operator '++' cannot be applied to operand of type '{args[0].Type}'"))
                .WithStaticMethod(FlType.OperatorPreDecr, (args) => throw new UnsupportedOperandException($"Operator '--' cannot be applied to operand of type '{args[0].Type}'"))
                .WithStaticMethod(FlType.OperatorPostDecr, (args) => throw new UnsupportedOperandException($"Operator '--' cannot be applied to operand of type '{args[0].Type}'"))
                
                .WithStaticMethod(FlType.OperatorAdd, (args) => throw new UnsupportedOperandException($"Operator '+' cannot be applied to operands of type '{args[1].Type}' and '{args[0].Type}'"))
                .WithStaticMethod(FlType.OperatorSub, (args) => throw new UnsupportedOperandException($"Operator '-' cannot be applied to operands of type '{args[1].Type}' and '{args[0].Type}'"))
                .WithStaticMethod(FlType.OperatorMult, (args) => throw new UnsupportedOperandException($"Operator '*' cannot be applied to operands of type '{args[1].Type}' and '{args[0].Type}'"))
                .WithStaticMethod(FlType.OperatorDiv, (args) => throw new UnsupportedOperandException($"Operator '/' cannot be applied to operands of type '{args[1].Type}' and '{args[0].Type}'"))

                .WithStaticMethod(FlType.OperatorAssign, (args) => throw new UnsupportedOperandException($"Operator '=' cannot be applied to operands of type '{args[0].Type}' and '{args[1].Type}'"))
                .WithStaticMethod(FlType.OperatorAddAndAssign, (args) => throw new UnsupportedOperandException($"Operator '+=' cannot be applied to operands of type '{args[1].Type}' and '{args[0].Type}'"))
                .WithStaticMethod(FlType.OperatorSubAndAssign, (args) => throw new UnsupportedOperandException($"Operator '-=' cannot be applied to operands of type '{args[1].Type}' and '{args[0].Type}'"))
                .WithStaticMethod(FlType.OperatorMultAndAssign, (args) => throw new UnsupportedOperandException($"Operator '*=' cannot be applied to operands of type '{args[1].Type}' and '{args[0].Type}'"))
                .WithStaticMethod(FlType.OperatorDivAndAssing, (args) => throw new UnsupportedOperandException($"Operator '/=' cannot be applied to operands of type '{args[1].Type}' and '{args[0].Type}'"))
                
                .WithStaticMethod(FlType.OperatorEquals, (args) => new FlBool(args[0].RawValue.Equals(args[1].RawValue)))
                .WithStaticMethod(FlType.OperatorNot, (args) => throw new UnsupportedOperandException($"Operator '!' cannot be applied to operand of type '{args[0].Type}'"))
                .WithStaticMethod(FlType.OperatorGt, (args) => throw new UnsupportedOperandException($"Operator '>' cannot be applied to operands of type '{args[1].Type}' and '{args[0].Type}'"))
                .WithStaticMethod(FlType.OperatorGte, (args) => throw new UnsupportedOperandException($"Operator '>=' cannot be applied to operands of type '{args[1].Type}' and '{args[0].Type}'"))
                .WithStaticMethod(FlType.OperatorLt, (args) => throw new UnsupportedOperandException($"Operator '<' cannot be applied to operands of type '{args[1].Type}' and '{args[0].Type}'"))
                .WithStaticMethod(FlType.OperatorLte, (args) => throw new UnsupportedOperandException($"Operator '<=' cannot be applied to operands of type '{args[1].Type}' and '{args[0].Type}'"))
            ;

            // Build
            return new FlObjectType(builder.Build());
        }
    }
}
