using System;

namespace Common.Core.DependencyInjection
{
    public class ServiceLocateAttribute : Attribute
    {
        private Type _iService;
        private ServiceType _serviceType;

        public ServiceLocateAttribute(Type iService, ServiceType type = ServiceType.Transient)
        {
            _iService = iService;
            _serviceType = type;
        }

        public Type IService { get => _iService; set => _iService = value; }

        public ServiceType ServiceType { get => _serviceType; set => _serviceType = value; }
    }
}
