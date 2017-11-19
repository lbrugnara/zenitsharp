// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fl.Engine.Symbols
{
    public class FlObject
    {
        protected ObjectType _ObjectType;
        protected object _Value;
        protected List<Symbol> _Refs;

        public FlObject(ObjectType type, object result)
        {
            _ObjectType = type;
            _Value = result;
            _Refs = new List<Symbol>();
        }
        
        public ObjectType Type { get => _ObjectType; set => _ObjectType = value; }

        public object Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
            }
        }

        public Symbol Owner => _Refs.FirstOrDefault();

        public void RefInc(Symbol s)
        {
            if (_Value is FlObject)
                _Refs.Add(s);
        }

        public void RefDec(Symbol s)
        {
            if (_Value is FlObject)
                _Refs.Remove(s);
        }

        public string AsString => _Value.ToString();

        public int AsInt => int.Parse(AsString);

        public double AsDouble => double.Parse(AsString);

        public decimal AsDecimal => decimal.Parse(AsString);

        public bool AsBool => bool.Parse(AsString);

        public FlCallable AsCallable => _Value as FlCallable;

        public FlNamespace AsNamespace => _Value as FlNamespace;

        public bool IsInt => _ObjectType == ObjectType.Integer && int.TryParse(AsString, out _);

        public bool IsDouble => _ObjectType == ObjectType.Double && double.TryParse(AsString, out _);

        public bool IsDecimal => _ObjectType == ObjectType.Decimal && decimal.TryParse(AsString, out _);

        public bool IsBool => _ObjectType == ObjectType.Boolean && bool.TryParse(AsString, out _);

        public bool IsNull => _ObjectType == ObjectType.Null && Value == null;

        public bool IsCallable => _ObjectType == ObjectType.Function && Value != null && Value is FlCallable;

        public bool IsNamespace => _ObjectType == ObjectType.Namespace && Value != null && Value is FlNamespace;

        public override string ToString()
        {
            switch (_ObjectType)
            {
                case ObjectType.Integer:
                    return $"{AsInt}";
                case ObjectType.Double:
                    return $"{AsDouble}";
                case ObjectType.Decimal:
                    return $"{AsDecimal}";
                case ObjectType.Boolean:
                    return $"{AsBool.ToString().ToLower()}";
                case ObjectType.String:
                    return $"{AsString}";
                case ObjectType.Function:
                    return $"fn {AsCallable?.Name}";
                case ObjectType.Namespace:
                    return $"namespace {AsNamespace.FullName}";
                case ObjectType.Null:
                    return $"null";                
            }
            throw new Exception("This is odd");
        }

        public virtual string ToDebugStr()
        {
            switch (_ObjectType)
            {
                case ObjectType.Integer:
                    return $"{AsInt} (int)";
                case ObjectType.Double:
                    return $"{AsDouble} (double)";
                case ObjectType.Decimal:
                    return $"{AsDecimal} (decimal)";
                case ObjectType.Boolean:
                    return $"{AsBool.ToString().ToLower()} (bool)";
                case ObjectType.String:
                    return $"{AsString} (string)";
                case ObjectType.Function:
                    return $"fn {AsCallable?.Name}";
                case ObjectType.Namespace:
                    return $"namespace {AsNamespace.FullName}";
                case ObjectType.Null:
                    return $"(null)";
            }
            throw new Exception("This is odd");
        }
    }
}
