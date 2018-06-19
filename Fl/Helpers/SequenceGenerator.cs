// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fl.Helpers
{
    class SequenceGenerator
    {
        private int[] names = new int[] { -1, -1, -1, -1, -1, -1, -1 };

        public string Generate()
        {
            // Poor's man type name generator
            for (var i = 0; i < this.names.Length; i++)
            {
                if (this.names[i] < 25)
                {
                    this.names[i] += 1;
                    return string.Join("", this.names.Where(n => n > -1).Select(n => (char)('A' + n)));
                }
                this.names[i] = 0;
            }
            throw new System.Exception("Carry Overflow");
        }
    }
}
