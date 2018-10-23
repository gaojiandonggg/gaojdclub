using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GaoJD.Club.Core.Extensions
{
    public static class ServiceCollectionExtension
    {

        public static T GetServiceCollection<T>(this IServiceCollection services)
        {
            return (T)services.LastOrDefault(p => p.ServiceType == typeof(T))?.ImplementationInstance;
        }

        public static object GetServiceCollection(this IServiceCollection services, Type type)
        {
            return services.LastOrDefault(p => p.ServiceType == type)?.ImplementationInstance;
        }
    }
}
