using System;

namespace Common.Core.AOP
{
    // If AOPInterception used, no need ServiceLocate for DI
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
