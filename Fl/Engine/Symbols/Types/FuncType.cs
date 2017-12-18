// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Objects;
using System;

namespace Fl.Engine.Symbols.Types
{
    public class FuncType : ObjectType
    {
        private static FuncType _Instance;

        private FuncType() { }

        public static FuncType Value => _Instance != null ? _Instance : (_Instance = new FuncType());

        public override string Name => "func";

        public override string ClassName => "func";

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
