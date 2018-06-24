// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Semantics.Types
{
    public class Method : Function
    {
        public Class DefiningClass { get; private set; }

        public Method()
            : base()
        {
        }

        public Method(Class clasz)
            : base()
        {
            this.DefiningClass = clasz;
        }

        public Method(Class clasz, Type returnType, params Type[] parametersTypes)
            : base(returnType, parametersTypes)
        {
            this.DefiningClass = clasz;
        }

        public void SetDefiningClass(Class clasz)
        {
            this.DefiningClass = clasz;
        }
    }
}
