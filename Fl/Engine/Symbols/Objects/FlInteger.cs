// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Types;
using System.Collections.Generic;

namespace Fl.Engine.Symbols.Objects
{
    public class FlInteger : FlObject
    {
        #region Private Fields
        private int _RawValue;
        #endregion

        #region Constructor
        public FlInteger(int value)
        {
            _RawValue = value;
        }
        #endregion

        #region Public Properties
        public int Value { get => _RawValue; set => _RawValue = value; }
        #endregion

        #region FlObject implementation
        public override ObjectType ObjectType => IntegerType.Value;

        public override object RawValue => _RawValue;

        public override bool IsPrimitive => true;

        public override FlObject Clone()
        {
            return new FlInteger(_RawValue);
        }

        public override FlObject ConvertTo(ObjectType type)
        {
            if (type == IntegerType.Value)
            {
                return this.Clone();
            }
            else if (type == DoubleType.Value)
            {
                return new FlDouble((double)_RawValue);
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

        #endregion

        #region Assignment Operators

        public override void Assign(FlObject n)
        {
            if (n.ObjectType == IntegerType.Value)
            {
                this.Value = (n as FlInteger).Value;
                return;
            }
            throw new SymbolException($"Operator '=' cannot be applied to operands of type '{n.ObjectType}' and '{this.ObjectType}'");
        }

        #endregion

        #region Arithmetics Operators
        public override FlObject PreIncrement()
        {
            this.Value += 1;
            return this;
        }

        public override FlObject PostIncrement()
        {
            var res = this.Clone();
            this.Value += 1;
            return res;
        }

        public override FlObject PreDecrement()
        {
            this.Value -= 1;
            return this;
        }

        public override FlObject PostDecrement()
        {
            var res = this.Clone();
            this.Value -= 1;
            return res;
        }

        public override FlObject Add(FlObject n)
        {
            if (n.ObjectType == IntegerType.Value)
            {
                return new FlInteger(this.Value + (int)n.RawValue);
            }
            else if (n.ObjectType == StringType.Value)
            {
                return new FlString(this.RawValue.ToString() + n.RawValue.ToString());
            }
            throw new SymbolException($"Operator '+' cannot be applied to operands of type '{n.ObjectType}' and '{this.ObjectType}'");
        }

        public override FlObject Substract(FlObject n)
        {
            if (n.ObjectType == IntegerType.Value)
            {
                return new FlInteger(this.Value - (int)n.RawValue);
            }
            throw new SymbolException($"Operator '-' cannot be applied to operands of type '{n.ObjectType}' and '{this.ObjectType}'");
        }

        public override FlObject Multiply(FlObject n)
        {
            if (n.ObjectType == IntegerType.Value)
            {
                return new FlInteger(this.Value * (int)n.RawValue);
            }
            throw new SymbolException($"Operator '*' cannot be applied to operands of type '{n.ObjectType}' and '{this.ObjectType}'");
        }

        public override FlObject Divide(FlObject n)
        {
            if (n.ObjectType == IntegerType.Value)
            {
                return new FlInteger(this.Value / (int)n.RawValue);
            }
            throw new SymbolException($"Operator '/' cannot be applied to operands of type '{n.ObjectType}' and '{this.ObjectType}'");
        }

        public override void AddAndAssign(FlObject n)
        {
            if (n.ObjectType == IntegerType.Value)
            {
                this.Value += (n as FlInteger).Value;
                return;
            }
            throw new SymbolException($"Operator '+=' cannot be applied to operands of type '{n.ObjectType}' and '{this.ObjectType}'");
        }

        public override void SubstractAndAssign(FlObject n)
        {
            if (n.ObjectType == IntegerType.Value)
            {
                this.Value -= (n as FlInteger).Value;
                return;
            }
            throw new SymbolException($"Operator '-=' cannot be applied to operands of type '{n.ObjectType}' and '{this.ObjectType}'");
        }

        public override void MultiplyAndAssing(FlObject n)
        {
            if (n.ObjectType == IntegerType.Value)
            {
                this.Value *= (n as FlInteger).Value;
                return;
            }
            throw new SymbolException($"Operator '*=' cannot be applied to operands of type '{n.ObjectType}' and '{this.ObjectType}'");
        }

        public override void DivideAndAssing(FlObject n)
        {
            if (n.ObjectType == IntegerType.Value)
            {
                this.Value /= (n as FlInteger).Value;
                return;
            }
            throw new SymbolException($"Operator '/=' cannot be applied to operands of type '{n.ObjectType}' and '{this.ObjectType}'");
        }

        public override FlObject Negative()
        {
            return new FlInteger(this.Value * -1);
        }
        #endregion

        #region Logical Operators

        public override FlBool Equals(FlObject n)
        {
            if (n.ObjectType == IntegerType.Value)
                return new FlBool(this.Value == (int)n.RawValue);
            return base.Equals(n);
        }

        public override FlBool GreatherThan(FlObject n)
        {
            if (n.ObjectType == IntegerType.Value)
                return new FlBool(this.Value > (int)n.RawValue);
            return base.GreatherThan(n);
        }

        public override FlBool GreatherThanEquals(FlObject n)
        {
            if (n.ObjectType == IntegerType.Value)
                return new FlBool(this.Value >= (int)n.RawValue);
            return base.GreatherThanEquals(n);
        }

        public override FlBool LesserThan(FlObject n)
        {
            if (n.ObjectType == IntegerType.Value)
                return new FlBool(this.Value < (int)n.RawValue);
            return base.LesserThan(n);
        }

        public override FlBool LesserThanEquals(FlObject n)
        {
            if (n.ObjectType == IntegerType.Value)
                return new FlBool(this.Value <= (int)n.RawValue);
            return base.LesserThanEquals(n);
        }
        #endregion
    }
}
