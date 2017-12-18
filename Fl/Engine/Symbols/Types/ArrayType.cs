// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Objects;
using System;

namespace Fl.Engine.Symbols.Types
{
    public class ArrayType : ObjectType
    {
        private static ArrayType _Instance;

        private ArrayType() { }

        public static ArrayType Value => _Instance != null ? _Instance : (_Instance = new ArrayType());

        public override string Name => "array";

        public override string ClassName => "array";

        public override object RawDefaultValue()
        {
            throw new NotImplementedException();
        }

        public override FlObject DefaultValue()
        {
            throw new NotImplementedException();
        }

        public override FlObject NewValue(object o)
        {
            throw new NotImplementedException();
        }
    }
}
