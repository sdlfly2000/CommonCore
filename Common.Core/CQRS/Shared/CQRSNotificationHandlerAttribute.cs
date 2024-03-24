using System;

namespace Common.Core.CQRS.Shared
{
    public class CQRSNotificationHandlerAttribute : Attribute
    {
        public CQRSNotificationHandlerAttribute(string notification, Type type)
        {
            Notification = notification;
            Type = type;
        }

        public string Notification { get; set; }
        public Type Type { get; set; }
    }
}
