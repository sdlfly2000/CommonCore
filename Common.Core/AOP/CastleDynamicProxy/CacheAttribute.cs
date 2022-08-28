using System;

namespace Common.Core.AOP.CastleDynamicProxy
{
    public class CacheAttribute : Attribute
    {
        public CacheAttribute(Type serviceType, Type serviceImplement)
        {
            Implement = serviceImplement;
            InterFace = serviceType;
        }

        public Type Implement { get; set; }
        public Type InterFace { get; set; }
    }
}
