// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fl.Engine.Symbols.Objects
{
    public abstract class FlObject
    {
        public abstract object RawValue { get; }

        public abstract bool IsPrimitive { get; }

        public abstract ObjectType ObjectType { get; }

        public abstract FlObject Clone();

        public abstract FlObject ConvertTo(ObjectType type);

        public virtual Symbol this[string membername] => throw new SymbolException($"{ObjectType} does not contain a definition of '{membername}'");

        #region Assignment Operators

        public virtual void Assign(FlObject n) => throw new SymbolException($"Operator '=' cannot be applied to operands of type '{n.ObjectType}' and '{this.ObjectType}'");

        #endregion

        #region Arithmetics Operators        

        public virtual FlObject PreIncrement() => throw new SymbolException($"Operator '++' cannot be applied to operand of type '{this.ObjectType}'");

        public virtual FlObject PostIncrement() => throw new SymbolException($"Operator '++' cannot be applied to operand of type '{this.ObjectType}'");

        public virtual FlObject PreDecrement() => throw new SymbolException($"Operator '--' cannot be applied to operand of type '{this.ObjectType}'");

        public virtual FlObject PostDecrement() => throw new SymbolException($"Operator '--' cannot be applied to operand of type '{this.ObjectType}'");

        public virtual FlObject Add(FlObject n) => throw new SymbolException($"Operator '+' cannot be applied to operands of type '{n.ObjectType}' and '{this.ObjectType}'");

        public virtual FlObject Substract(FlObject n) => throw new SymbolException($"Operator '-' cannot be applied to operands of type '{n.ObjectType}' and '{this.ObjectType}'");

        public virtual FlObject Multiply(FlObject n) => throw new SymbolException($"Operator '*' cannot be applied to operands of type '{n.ObjectType}' and '{this.ObjectType}'");

        public virtual FlObject Divide(FlObject n) => throw new SymbolException($"Operator '/' cannot be applied to operands of type '{n.ObjectType}' and '{this.ObjectType}'");

        public virtual void AddAndAssign(FlObject n) => throw new SymbolException($"Operator '+=' cannot be applied to operands of type '{n.ObjectType}' and '{this.ObjectType}'");

        public virtual void SubstractAndAssign(FlObject n) => throw new SymbolException($"Operator '-=' cannot be applied to operands of type '{n.ObjectType}' and '{this.ObjectType}'");

        public virtual void MultiplyAndAssing(FlObject n) => throw new SymbolException($"Operator '*=' cannot be applied to operands of type '{n.ObjectType}' and '{this.ObjectType}'");

        public virtual void DivideAndAssing(FlObject n) => throw new SymbolException($"Operator '/=' cannot be applied to operands of type '{n.ObjectType}' and '{this.ObjectType}'");

        public virtual FlObject Negative() => throw new SymbolException($"Operator '-' cannot be applied to operand of type '{this.ObjectType}'");

        #endregion

        #region Logical Operators

        public virtual FlBool Equals(FlObject n) => new FlBool(this.RawValue.Equals(n.RawValue));

        public virtual FlObject Not() => throw new SymbolException($"Operator '!' cannot be applied to operand of type '{this.ObjectType}'");

        public virtual FlBool GreatherThan(FlObject n) => throw new SymbolException($"Operator '>' cannot be applied to operands of type '{n.ObjectType}' and '{this.ObjectType}'");

        public virtual FlBool GreatherThanEquals(FlObject n) => throw new SymbolException($"Operator '>=' cannot be applied to operands of type '{n.ObjectType}' and '{this.ObjectType}'");

        public virtual FlBool LesserThan(FlObject n) => throw new SymbolException($"Operator '<' cannot be applied to operands of type '{n.ObjectType}' and '{this.ObjectType}'");

        public virtual FlBool LesserThanEquals(FlObject n) => throw new SymbolException($"Operator '<=' cannot be applied to operands of type '{n.ObjectType}' and '{this.ObjectType}'");

        #endregion

        public override string ToString()
        {
            return RawValue.ToString();
        }

        public virtual string ToDebugStr()
        {
            return $"{RawValue} ({ObjectType})";
        }
    }
}
