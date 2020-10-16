using System;

namespace Common.Core.DependencyInjection
{
    public class ServiceLocateAttribute : Attribute
    {
        private Type _iService;

        public ServiceLocateAttribute(Type iService)
        {
            _iService = iService;
        }

        public Type IService { get => _iService; set => _iService = value; }
    }
}
