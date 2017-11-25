// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Types;

namespace Fl.Engine.Symbols.Objects
{
    public class FlString : FlObject
    {
        private string _RawValue;

        public FlString(string value)
        {
            _RawValue = value;
        }

        public override ObjectType ObjectType => StringType.Value;

        public override object RawValue => _RawValue;

        public string Value { get => _RawValue; set => _RawValue = value; }

        public override bool IsPrimitive => true;

        public override FlObject Clone()
        {
            return new FlString(_RawValue);
        }

        public override FlObject ConvertTo(ObjectType type)
        {
            if (type == StringType.Value)
            {
                return this.Clone();
            }
            throw new CastException($"Cannot convert type {ObjectType} to {type}");
        }

        public override string ToDebugStr()
        {
            return $"\"{RawValue}\" ({ObjectType})";
        }

        #region Assignment Operators

        public override void Assign(FlObject n)
        {
            if (n.ObjectType == StringType.Value)
            {
                this.Value = (n as FlString).Value;
                return;
            }
            base.Assign(n);
        }

        #endregion

        #region Arithmetics Operators

        public override FlObject Add(FlObject n)
        {
            if (n.ObjectType == StringType.Value)
            {
                return new FlString(this.RawValue.ToString() + n.RawValue.ToString());
            }
            return base.Add(n);
        }
        
        public override void AddAndAssign(FlObject n)
        {
            if (n.ObjectType == StringType.Value)
            {
                this.Value += (n as FlString).Value;
                return;
            }
            base.Add(n);
        }
        #endregion

        #region Logical Operators
        
        public override FlBool GreatherThan(FlObject n)
        {
            if (n.ObjectType == StringType.Value)
                return new FlBool(this.Value.CompareTo((n as FlString).Value) > 0);
            return base.GreatherThan(n);
        }

        public override FlBool GreatherThanEquals(FlObject n)
        {
            if (n.ObjectType == StringType.Value)
                return new FlBool(this.Value.CompareTo((n as FlString).Value) >= 0);
            return base.GreatherThanEquals(n);
        }

        public override FlBool LesserThan(FlObject n)
        {
            if (n.ObjectType == StringType.Value)
                return new FlBool(this.Value.CompareTo((n as FlString).Value) < 0);
            return base.LesserThan(n);
        }

        public override FlBool LesserThanEquals(FlObject n)
        {
            if (n.ObjectType == StringType.Value)
                return new FlBool(this.Value.CompareTo((n as FlString).Value) <= 0);
            return base.LesserThanEquals(n);
        }
        #endregion
    }
}
