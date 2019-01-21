// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;
using System.Linq;

namespace Fl.Helpers
{
    class NameGenerator
    {
        private int[] seed = new int[] { -1, -1, -1, -1, -1, -1, -1 };

        public static readonly Lazy<NameGenerator> lazyInstance = new Lazy<NameGenerator>(() => new NameGenerator(), true);

        private NameGenerator()
        {
        }

        public static NameGenerator Instance => lazyInstance.Value;

        public void Reset()
        {
            for (var i = 0; i < this.seed.Length; i++)
                this.seed[i] = -1;
        }

        public string Next()
        {
            // Poor's man type name generator
            for (var i = 0; i < this.seed.Length; i++)
            {
                if (this.seed[i] < 25)
                {
                    this.seed[i] += 1;
                    return string.Join("", this.seed.Where(n => n > -1).Select(n => (char)('a' + n)));
                }
                this.seed[i] = 0;
            }
            throw new System.Exception("Carry Overflow");
        }
    }
}
