// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Types;
using System;

namespace Fl.Engine.Symbols
{
    public class FlOperand
    {
        private FlObject _Ref;

        public FlOperand(FlObject obj)
        {
            _Ref = obj ?? throw new ArgumentNullException(nameof(obj));
        }

        #region Equality
        public FlBoolean Equals(FlObject n)
        {
            if (_Ref.ObjectType == IntegerType.Value && n.ObjectType == IntegerType.Value)
            {
                return new FlBoolean((int)_Ref.RawValue == (int)n.RawValue);
            }
            else if (_Ref.ObjectType == DoubleType.Value && n.ObjectType == DoubleType.Value)
            {
                return new FlBoolean((double)_Ref.RawValue == (double)n.RawValue);
            }
            else if (_Ref.ObjectType == DecimalType.Value && n.ObjectType == DecimalType.Value)
            {
                return new FlBoolean((decimal)_Ref.RawValue == (decimal)n.RawValue);
            }
            return new FlBoolean(_Ref.RawValue.Equals(n.RawValue));
        }
        #endregion

        public void Assign(FlObject n)
        {
            if (_Ref.ObjectType == IntegerType.Value && n.ObjectType == IntegerType.Value)
            {
                (_Ref as FlInteger).Value = (n as FlInteger).Value;
                return;
            }
            else if (_Ref.ObjectType == DoubleType.Value && n.ObjectType == DoubleType.Value)
            {
                (_Ref as FlDouble).Value = (n as FlDouble).Value;
                return;
            }
            else if (_Ref.ObjectType == DecimalType.Value && n.ObjectType == DecimalType.Value)
            {
                (_Ref as FlDecimal).Value = (n as FlDecimal).Value;
                return;
            }
            throw new Exception($"Operator '=' cannot be applied to operands of type '{n.ObjectType}' and '{_Ref.ObjectType}'");
        }

        #region Arithmetic
        public FlObject PreIncrement()
        {
            if (_Ref.ObjectType == IntegerType.Value)
            {
                (_Ref as FlInteger).Value += 1;
                return _Ref;
            }
            else if (_Ref.ObjectType == DoubleType.Value)
            {
                (_Ref as FlDouble).Value += 1.0;
                return _Ref;
            }
            else if (_Ref.ObjectType == DecimalType.Value)
            {
                (_Ref as FlDecimal).Value += 1.0M;
                return _Ref;
            }
            throw new Exception($"Operator '++' cannot be applied to operand of type '{_Ref.ObjectType}'");
        }

        public FlObject PostIncrement()
        {
            if (_Ref.ObjectType == IntegerType.Value)
            {
                var res = _Ref.Clone();
                (_Ref as FlInteger).Value += 1;
                return res;
            }
            else if (_Ref.ObjectType == DoubleType.Value)
            {
                var res = _Ref.Clone();
                (_Ref as FlDouble).Value += 1.0;
                return res;
            }
            else if (_Ref.ObjectType == DecimalType.Value)
            {
                var res = _Ref.Clone();
                (_Ref as FlDecimal).Value += 1.0M;
                return res;
            }
            throw new Exception($"Operator '++' cannot be applied to operand of type '{_Ref.ObjectType}'");
        }

        public FlObject PreDecrement()
        {
            if (_Ref.ObjectType == IntegerType.Value)
            {
                (_Ref as FlInteger).Value -= 1;
                return _Ref;
            }
            else if (_Ref.ObjectType == DoubleType.Value)
            {
                (_Ref as FlDouble).Value -= 1.0;
                return _Ref;
            }
            else if (_Ref.ObjectType == DecimalType.Value)
            {
                (_Ref as FlDecimal).Value -= 1.0M;
                return _Ref;
            }
            throw new Exception($"Operator '--' cannot be applied to operand of type '{_Ref.ObjectType}'");
        }

        public FlObject PostDecrement()
        {
            if (_Ref.ObjectType == IntegerType.Value)
            {
                var res = _Ref.Clone();
                (_Ref as FlInteger).Value -= 1;
                return res;
            }
            else if (_Ref.ObjectType == DoubleType.Value)
            {
                var res = _Ref.Clone();
                (_Ref as FlDouble).Value -= 1.0;
                return res;
            }
            else if (_Ref.ObjectType == DecimalType.Value)
            {
                var res = _Ref.Clone();
                (_Ref as FlDecimal).Value -= 1.0M;
                return res;
            }
            throw new Exception($"Operator '--' cannot be applied to operand of type '{_Ref.ObjectType}'");
        }

        public FlObject Add(FlObject n)
        {
            if (_Ref.ObjectType == IntegerType.Value && n.ObjectType == IntegerType.Value)
            {
                return new FlInteger((int)_Ref.RawValue + (int)n.RawValue);
            }
            else if (_Ref.ObjectType == DoubleType.Value && n.ObjectType == DoubleType.Value)
            {
                return new FlDouble((double)_Ref.RawValue + (double)n.RawValue);
            }
            else if (_Ref.ObjectType == DecimalType.Value && n.ObjectType == DecimalType.Value)
            {
                return new FlDecimal((decimal)_Ref.RawValue + (decimal)n.RawValue);
            }
            else if (_Ref.ObjectType == StringType.Value || n.ObjectType == StringType.Value)
            {
                return new FlString(_Ref.RawValue.ToString() + n.RawValue.ToString());
            }
            throw new Exception($"Operator '+' cannot be applied to operands of type '{n.ObjectType}' and '{_Ref.ObjectType}'");
        }

        public FlObject Substract(FlObject n)
        {
            if (_Ref.ObjectType == IntegerType.Value && n.ObjectType == IntegerType.Value)
            {
                return new FlInteger((int)_Ref.RawValue - (int)n.RawValue);
            }
            else if (_Ref.ObjectType == DoubleType.Value && n.ObjectType == DoubleType.Value)
            {
                return new FlDouble((double)_Ref.RawValue - (double)n.RawValue);
            }
            else if (_Ref.ObjectType == DecimalType.Value && n.ObjectType == DecimalType.Value)
            {
                return new FlDecimal((decimal)_Ref.RawValue - (decimal)n.RawValue);
            }
            throw new Exception($"Operator '-' cannot be applied to operands of type '{n.ObjectType}' and '{_Ref.ObjectType}'");
        }

        public FlObject Multiply(FlObject n)
        {
            if (_Ref.ObjectType == IntegerType.Value && n.ObjectType == IntegerType.Value)
            {
                return new FlInteger((int)_Ref.RawValue * (int)n.RawValue);
            }
            else if (_Ref.ObjectType == DoubleType.Value && n.ObjectType == DoubleType.Value)
            {
                return new FlDouble((double)_Ref.RawValue * (double)n.RawValue);
            }
            else if (_Ref.ObjectType == DecimalType.Value && n.ObjectType == DecimalType.Value)
            {
                return new FlDecimal((decimal)_Ref.RawValue * (decimal)n.RawValue);
            }
            throw new Exception($"Operator '*' cannot be applied to operands of type '{n.ObjectType}' and '{_Ref.ObjectType}'");
        }

        public FlObject Divide(FlObject n)
        {
            if (_Ref.ObjectType == IntegerType.Value && n.ObjectType == IntegerType.Value)
            {
                return new FlInteger((int)_Ref.RawValue * (int)n.RawValue);
            }
            else if (_Ref.ObjectType == DoubleType.Value && n.ObjectType == DoubleType.Value)
            {
                return new FlDouble((double)_Ref.RawValue * (double)n.RawValue);
            }
            else if (_Ref.ObjectType == DecimalType.Value && n.ObjectType == DecimalType.Value)
            {
                return new FlDecimal((decimal)_Ref.RawValue * (decimal)n.RawValue);
            }
            throw new Exception($"Operator '/' cannot be applied to operands of type '{n.ObjectType}' and '{_Ref.ObjectType}'");
        }

        public void AddAndAssign(FlObject n)
        {
            if (_Ref.ObjectType == IntegerType.Value && n.ObjectType == IntegerType.Value)
            {
                (_Ref as FlInteger).Value += (n as FlInteger).Value;
            }
            else if (_Ref.ObjectType == DoubleType.Value && n.ObjectType == DoubleType.Value)
            {
                (_Ref as FlDouble).Value += (n as FlDouble).Value;
            }
            else if (_Ref.ObjectType == DecimalType.Value && n.ObjectType == DecimalType.Value)
            {
                (_Ref as FlDecimal).Value += (n as FlDecimal).Value;
            }
            throw new Exception($"Operator '+=' cannot be applied to operands of type '{n.ObjectType}' and '{_Ref.ObjectType}'");
        }

        public void SubstractAndAssign(FlObject n)
        {
            if (_Ref.ObjectType == IntegerType.Value && n.ObjectType == IntegerType.Value)
            {
                (_Ref as FlInteger).Value -= (n as FlInteger).Value;
            }
            else if (_Ref.ObjectType == DoubleType.Value && n.ObjectType == DoubleType.Value)
            {
                (_Ref as FlDouble).Value -= (n as FlDouble).Value;
            }
            else if (_Ref.ObjectType == DecimalType.Value && n.ObjectType == DecimalType.Value)
            {
                (_Ref as FlDecimal).Value -= (n as FlDecimal).Value;
            }
            throw new Exception($"Operator '-=' cannot be applied to operands of type '{n.ObjectType}' and '{_Ref.ObjectType}'");
        }

        public void MultiplyAndAssing(FlObject n)
        {
            if (_Ref.ObjectType == IntegerType.Value && n.ObjectType == IntegerType.Value)
            {
                (_Ref as FlInteger).Value *= (n as FlInteger).Value;
            }
            else if (_Ref.ObjectType == DoubleType.Value && n.ObjectType == DoubleType.Value)
            {
                (_Ref as FlDouble).Value *= (n as FlDouble).Value;
            }
            else if (_Ref.ObjectType == DecimalType.Value && n.ObjectType == DecimalType.Value)
            {
                (_Ref as FlDecimal).Value *= (n as FlDecimal).Value;
            }
            throw new Exception($"Operator '*=' cannot be applied to operands of type '{n.ObjectType}' and '{_Ref.ObjectType}'");
        }

        public void DivideAndAssing(FlObject n)
        {
            if (_Ref.ObjectType == IntegerType.Value && n.ObjectType == IntegerType.Value)
            {
                (_Ref as FlInteger).Value /= (n as FlInteger).Value;
            }
            else if (_Ref.ObjectType == DoubleType.Value && n.ObjectType == DoubleType.Value)
            {
                (_Ref as FlDouble).Value /= (n as FlDouble).Value;
            }
            else if (_Ref.ObjectType == DecimalType.Value && n.ObjectType == DecimalType.Value)
            {
                (_Ref as FlDecimal).Value /= (n as FlDecimal).Value;
            }
            throw new Exception($"Operator '*=' cannot be applied to operands of type '{n.ObjectType}' and '{_Ref.ObjectType}'");
        }

        public FlObject Negative()
        {
            if (_Ref.ObjectType == IntegerType.Value)
            {
                return new FlInteger((int)_Ref.RawValue * -1);
            }
            else if (_Ref.ObjectType == DoubleType.Value)
            {
                return new FlDouble((double)_Ref.RawValue * -1.0);
            }
            else if (_Ref.ObjectType == DecimalType.Value)
            {
                return new FlDecimal((decimal)_Ref.RawValue * -1.0M);
            }
            throw new Exception($"Operator '-' cannot be applied to operand of type '{_Ref.ObjectType}'");
        }
        #endregion

        #region Inequality
        public FlBoolean GreatherThan(FlObject n)
        {
            if (_Ref.ObjectType == DecimalType.Value || n.ObjectType == DecimalType.Value)
                return new FlBoolean((decimal)_Ref.RawValue > (decimal)n.RawValue);

            if (_Ref.ObjectType == DoubleType.Value || n.ObjectType == DoubleType.Value)
                return new FlBoolean((double)_Ref.RawValue > (double)n.RawValue);

            if ((_Ref.ObjectType == IntegerType.Value || n.ObjectType == IntegerType.Value))
                return new FlBoolean((int)_Ref.RawValue > (int)n.RawValue);

            if (_Ref.ObjectType == StringType.Value && n.ObjectType == StringType.Value)
                return new FlBoolean((_Ref as FlString).Value.CompareTo((n as FlString).Value) > 0);
            throw new Exception($"Operator '>' cannot be applied to operands of type '{n.ObjectType}' and '{_Ref.ObjectType}'");
        }

        public FlBoolean GreatherThanEquals(FlObject n)
        {
            if (_Ref.ObjectType == DecimalType.Value || n.ObjectType == DecimalType.Value)
                return new FlBoolean((decimal)_Ref.RawValue >= (decimal)n.RawValue);

            if (_Ref.ObjectType == DoubleType.Value || n.ObjectType == DoubleType.Value)
                return new FlBoolean((double)_Ref.RawValue >= (double)n.RawValue);

            if ((_Ref.ObjectType == IntegerType.Value || n.ObjectType == IntegerType.Value))
                return new FlBoolean((int)_Ref.RawValue >= (int)n.RawValue);

            if (_Ref.ObjectType == StringType.Value && n.ObjectType == StringType.Value)
                return new FlBoolean((_Ref as FlString).Value.CompareTo((n as FlString).Value) >= 0);
            throw new Exception($"Operator '>' cannot be applied to operands of type '{n.ObjectType}' and '{_Ref.ObjectType}'");
        }

        public FlBoolean LesserThan(FlObject n)
        {
            if (_Ref.ObjectType == DecimalType.Value || n.ObjectType == DecimalType.Value)
                return new FlBoolean((decimal)_Ref.RawValue < (decimal)n.RawValue);

            if (_Ref.ObjectType == DoubleType.Value || n.ObjectType == DoubleType.Value)
                return new FlBoolean((double)_Ref.RawValue < (double)n.RawValue);

            if ((_Ref.ObjectType == IntegerType.Value || n.ObjectType == IntegerType.Value))
                return new FlBoolean((int)_Ref.RawValue < (int)n.RawValue);

            if (_Ref.ObjectType == StringType.Value && n.ObjectType == StringType.Value)
                return new FlBoolean((_Ref as FlString).Value.CompareTo((n as FlString).Value) < 0);
            throw new Exception($"Operator '>' cannot be applied to operands of type '{n.ObjectType}' and '{_Ref.ObjectType}'");
        }

        public FlBoolean LesserThanEquals(FlObject n)
        {
            if (_Ref.ObjectType == DecimalType.Value || n.ObjectType == DecimalType.Value)
                return new FlBoolean((decimal)_Ref.RawValue <= (decimal)n.RawValue);

            if (_Ref.ObjectType == DoubleType.Value || n.ObjectType == DoubleType.Value)
                return new FlBoolean((double)_Ref.RawValue <= (double)n.RawValue);

            if ((_Ref.ObjectType == IntegerType.Value || n.ObjectType == IntegerType.Value))
                return new FlBoolean((int)_Ref.RawValue <= (int)n.RawValue);

            if (_Ref.ObjectType == StringType.Value && n.ObjectType == StringType.Value)
                return new FlBoolean((_Ref as FlString).Value.CompareTo((n as FlString).Value) <= 0);
            throw new Exception($"Operator '>' cannot be applied to operands of type '{n.ObjectType}' and '{_Ref.ObjectType}'");
        }
        #endregion
    }
}
