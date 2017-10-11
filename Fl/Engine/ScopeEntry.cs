// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine
{
    public class ScopeEntry
    {
        protected ScopeEntryType _DataType;
        protected StorageType _StorageType;
        protected object _Value;

        public ScopeEntry()
        {
            _DataType = ScopeEntryType.Null;
            _StorageType = StorageType.Variable;
            _Value = null;
        }

        public ScopeEntry(ScopeEntryType type, object result)
        {
            _DataType = type;
            _Value = result;
        }

        public ScopeEntry(ScopeEntryType type, StorageType modifier, object result)
            : this(type, result)
        {
            _StorageType = modifier;
        }
        
        public ScopeEntryType DataType => _DataType;

        public StorageType Modifier => _StorageType;

        public object Value => _Value;

        public string StrValue => _Value.ToString();

        public int IntValue => int.Parse(StrValue);

        public double DoubleValue => double.Parse(StrValue);

        public decimal DecimalValue => decimal.Parse(StrValue);

        public bool BoolValue => bool.Parse(StrValue);

        public FlCallable FuncValue => _Value as FlCallable;

        public FlNamespace NamespaceValue => _Value as FlNamespace;

        public bool IsInt => _DataType == ScopeEntryType.Integer && int.TryParse(StrValue, out _);

        public bool IsDouble => _DataType == ScopeEntryType.Double && double.TryParse(StrValue, out _);

        public bool IsDecimal => _DataType == ScopeEntryType.Decimal && decimal.TryParse(StrValue, out _);

        public bool IsBool => _DataType == ScopeEntryType.Boolean && bool.TryParse(StrValue, out _);

        public bool IsNull => _DataType == ScopeEntryType.Null && Value == null;

        public bool IsCallable => _DataType == ScopeEntryType.Function && Value != null && Value is FlCallable;

        public bool IsNamespace => _DataType == ScopeEntryType.Namespace && Value != null && Value is FlNamespace;

        public override string ToString()
        {
            switch (_DataType)
            {
                case ScopeEntryType.Integer:
                    return $"{IntValue}";
                case ScopeEntryType.Double:
                    return $"{DoubleValue}";
                case ScopeEntryType.Decimal:
                    return $"{DecimalValue}";
                case ScopeEntryType.Boolean:
                    return $"{BoolValue.ToString().ToLower()}";
                case ScopeEntryType.String:
                    return $"{StrValue}";
                case ScopeEntryType.Function:
                    return $"func {FuncValue?.Name}";
                case ScopeEntryType.Namespace:
                    return $"namespace {NamespaceValue.FullName}";
                case ScopeEntryType.Null:
                    return $"null";                
            }
            throw new Exception("This is odd");
        }

        public virtual string ToDebugStr()
        {
            switch (_DataType)
            {
                case ScopeEntryType.Integer:
                    return $"{IntValue} (int)";
                case ScopeEntryType.Double:
                    return $"{DoubleValue} (double)";
                case ScopeEntryType.Decimal:
                    return $"{DecimalValue} (decimal)";
                case ScopeEntryType.Boolean:
                    return $"{BoolValue.ToString().ToLower()} (bool)";
                case ScopeEntryType.String:
                    return $"{StrValue} (string)";
                case ScopeEntryType.Function:
                    return $"func {FuncValue?.Name}";
                case ScopeEntryType.Namespace:
                    return $"namespace {NamespaceValue.FullName}";
                case ScopeEntryType.Null:
                    return $"(null)";
            }
            throw new Exception("This is odd");
        }
    }
}
