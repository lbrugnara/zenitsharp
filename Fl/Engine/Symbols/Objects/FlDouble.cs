// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Types;

namespace Fl.Engine.Symbols.Objects
{
    public class FlDouble : FlObject
    {
        private double _RawValue;

        public FlDouble(double value)
        {
            _RawValue = value;
        }

        public override ObjectType ObjectType => DoubleType.Value;

        public override object RawValue => _RawValue;

        public double Value { get => _RawValue; set => _RawValue = value; }

        public override bool IsPrimitive => true;

        public override FlObject Clone()
        {
            return new FlDouble(_RawValue);
        }

        public override FlObject ConvertTo(ObjectType type)
        {
            if (type == DoubleType.Value) 
            {
                return this.Clone();
            }
            else if (type == IntegerType.Value)
            {
                return new FlInteger((int)_RawValue);
            }
            else if (type == DecimalType.Value)
            {
                return new FlDecimal((decimal)_RawValue);
            }
            else if (type == StringType.Value)
            {
                return new FlString(_RawValue.ToString());
            }
            throw new CastException($"Cannot convert type {ObjectType} to {type}");
        }

        #region Assignment Operators

        public override void Assign(FlObject n)
        {
            if (n.ObjectType == DoubleType.Value)
            {
                this.Value = (n as FlDouble).Value;
                return;
            }
            throw new SymbolException($"Operator '=' cannot be applied to operands of type '{n.ObjectType}' and '{this.ObjectType}'");
        }

        #endregion

        #region Arithmetics Operators
        public override FlObject PreIncrement()
        {
            this.Value += 1.0;
            return this;
        }

        public override FlObject PostIncrement()
        {
            var res = this.Clone();
            this.Value += 1.0;
            return res;
        }

        public override FlObject PreDecrement()
        {
            this.Value -= 1.0;
            return this;
        }

        public override FlObject PostDecrement()
        {
            var res = this.Clone();
            this.Value -= 1.0;
            return res;
        }

        public override FlObject Add(FlObject n)
        {
            if (n.ObjectType == DoubleType.Value)
            {
                return new FlDouble(this.Value + (double)n.RawValue);
            }
            throw new SymbolException($"Operator '+' cannot be applied to operands of type '{n.ObjectType}' and '{this.ObjectType}'");
        }

        public override FlObject Subtract(FlObject n)
        {
            if (n.ObjectType == DoubleType.Value)
            {
                return new FlDouble(this.Value - (double)n.RawValue);
            }
            throw new SymbolException($"Operator '-' cannot be applied to operands of type '{n.ObjectType}' and '{this.ObjectType}'");
        }

        public override FlObject Multiply(FlObject n)
        {
            if (n.ObjectType == DoubleType.Value)
            {
                return new FlDouble(this.Value * (double)n.RawValue);
            }
            throw new SymbolException($"Operator '*' cannot be applied to operands of type '{n.ObjectType}' and '{this.ObjectType}'");
        }

        public override FlObject Divide(FlObject n)
        {
            if (n.ObjectType == DoubleType.Value)
            {
                return new FlDouble(this.Value / (double)n.RawValue);
            }
            throw new SymbolException($"Operator '/' cannot be applied to operands of type '{n.ObjectType}' and '{this.ObjectType}'");
        }

        public override void AddAndAssign(FlObject n)
        {
            if (n.ObjectType == DoubleType.Value)
            {
                this.Value += (n as FlDouble).Value;
                return;
            }
            throw new SymbolException($"Operator '+=' cannot be applied to operands of type '{n.ObjectType}' and '{this.ObjectType}'");
        }

        public override void SubtractAndAssign(FlObject n)
        {
            if (n.ObjectType == DoubleType.Value)
            {
                this.Value -= (n as FlDouble).Value;
                return;
            }
            throw new SymbolException($"Operator '-=' cannot be applied to operands of type '{n.ObjectType}' and '{this.ObjectType}'");
        }

        public override void MultiplyAndAssing(FlObject n)
        {
            if (n.ObjectType == DoubleType.Value)
            {
                this.Value *= (n as FlDouble).Value;
                return;
            }
            throw new SymbolException($"Operator '*=' cannot be applied to operands of type '{n.ObjectType}' and '{this.ObjectType}'");
        }

        public override void DivideAndAssing(FlObject n)
        {
            if (n.ObjectType == DoubleType.Value)
            {
                this.Value /= (n as FlDouble).Value;
                return;
            }
            throw new SymbolException($"Operator '/=' cannot be applied to operands of type '{n.ObjectType}' and '{this.ObjectType}'");
        }

        public override FlObject Negate()
        {
            return new FlDouble(this.Value * -1.0);
        }
        #endregion

        #region Logical Operators

        public override FlBool Equals(FlObject n)
        {
            if (n.ObjectType == DoubleType.Value)
                return new FlBool(this.Value == (double)n.RawValue);
            return base.Equals(n);
        }

        public override FlBool GreatherThan(FlObject n)
        {
            if (n.ObjectType == DoubleType.Value)
                return new FlBool(this.Value > (double)n.RawValue);
            return base.GreatherThan(n);
        }

        public override FlBool GreatherThanEquals(FlObject n)
        {
            if (n.ObjectType == DoubleType.Value)
                return new FlBool(this.Value >= (double)n.RawValue);
            return base.GreatherThanEquals(n);
        }

        public override FlBool LesserThan(FlObject n)
        {
            if (n.ObjectType == DoubleType.Value)
                return new FlBool(this.Value < (double)n.RawValue);
            return base.LesserThan(n);
        }

        public override FlBool LesserThanEquals(FlObject n)
        {
            if (n.ObjectType == DoubleType.Value)
                return new FlBool(this.Value <= (double)n.RawValue);
            return base.LesserThanEquals(n);
        }
        #endregion
    }
}
