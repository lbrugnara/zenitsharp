
// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Symbols.Types;
using System.Collections.Generic;

namespace Fl.Semantics.Symbols.Containers
{
    public class Package : Block, IPackage
    {
        public Package(string name)
            : this (name, null)
        {            
        }

        public Package(string name, IContainer parent)
            : base (name, parent)
        {
        }

        #region IPackage implementation        

        #endregion
    }
}
