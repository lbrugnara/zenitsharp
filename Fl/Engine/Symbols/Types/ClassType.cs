// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Objects;
using System;

namespace Fl.Engine.Symbols.Types
{
    public class ClassType : ObjectType
    {
        private static ClassType _Instance;

        private ClassType() { }

        public static ClassType Value => _Instance != null ? _Instance : (_Instance = new ClassType());

        public override string Name => "class";

        public override string ClassName => "Class";

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
