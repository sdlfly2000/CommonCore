using System;

namespace Common.Core.AOP
{
    public class AOPInterceptionAttribute : Attribute
    {
        public AOPInterceptionAttribute(Type serviceType, Type serviceImplement)
        {
            Implement = serviceImplement;
            InterFace = serviceType;
        }

        public Type Implement { get; set; }
        public Type InterFace { get; set; }
    }
}
