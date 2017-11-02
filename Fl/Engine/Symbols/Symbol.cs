// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Symbols
{
    public class Symbol
    {
        protected SymbolType _DataType;
        protected StorageType _StorageType;
        protected object _Value;

        public Symbol()
        {
            _DataType = SymbolType.Null;
            _StorageType = StorageType.Variable;
            _Value = null;
        }

        public Symbol(SymbolType type, object result)
        {
            _DataType = type;
            _Value = result;
        }

        public Symbol(SymbolType type, StorageType modifier, object result)
            : this(type, result)
        {
            _StorageType = modifier;
        }
        
        public SymbolType DataType => _DataType;

        public StorageType Storage => _StorageType;

        public object Value => _Value;

        public string AsString => _Value.ToString();

        public int AsInt => int.Parse(AsString);

        public double AsDouble => double.Parse(AsString);

        public decimal AsDecimal => decimal.Parse(AsString);

        public bool AsBool => bool.Parse(AsString);

        public FlCallable AsCallable => _Value as FlCallable;

        public FlNamespace AsNamespace => _Value as FlNamespace;

        public bool IsInt => _DataType == SymbolType.Integer && int.TryParse(AsString, out _);

        public bool IsDouble => _DataType == SymbolType.Double && double.TryParse(AsString, out _);

        public bool IsDecimal => _DataType == SymbolType.Decimal && decimal.TryParse(AsString, out _);

        public bool IsBool => _DataType == SymbolType.Boolean && bool.TryParse(AsString, out _);

        public bool IsNull => _DataType == SymbolType.Null && Value == null;

        public bool IsCallable => _DataType == SymbolType.Function && Value != null && Value is FlCallable;

        public bool IsNamespace => _DataType == SymbolType.Namespace && Value != null && Value is FlNamespace;

        public override string ToString()
        {
            switch (_DataType)
            {
                case SymbolType.Integer:
                    return $"{AsInt}";
                case SymbolType.Double:
                    return $"{AsDouble}";
                case SymbolType.Decimal:
                    return $"{AsDecimal}";
                case SymbolType.Boolean:
                    return $"{AsBool.ToString().ToLower()}";
                case SymbolType.String:
                    return $"{AsString}";
                case SymbolType.Function:
                    return $"func {AsCallable?.Name}";
                case SymbolType.Namespace:
                    return $"namespace {AsNamespace.FullName}";
                case SymbolType.Null:
                    return $"null";                
            }
            throw new Exception("This is odd");
        }

        public virtual string ToDebugStr()
        {
            switch (_DataType)
            {
                case SymbolType.Integer:
                    return $"{AsInt} (int)";
                case SymbolType.Double:
                    return $"{AsDouble} (double)";
                case SymbolType.Decimal:
                    return $"{AsDecimal} (decimal)";
                case SymbolType.Boolean:
                    return $"{AsBool.ToString().ToLower()} (bool)";
                case SymbolType.String:
                    return $"{AsString} (string)";
                case SymbolType.Function:
                    return $"func {AsCallable?.Name}";
                case SymbolType.Namespace:
                    return $"namespace {AsNamespace.FullName}";
                case SymbolType.Null:
                    return $"(null)";
            }
            throw new Exception("This is odd");
        }
    }
}
