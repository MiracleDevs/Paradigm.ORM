using System;
using Microsoft.Extensions.DependencyInjection;

namespace Paradigm.ORM.Data.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="IServiceProvider"/> interface.
    /// </summary>
    internal static class ServiceProviderExtensions
    {
        /// <summary>
        /// Gets the service object of the specified type, or provided instance if the service can not be resolved,
        /// or the service provider is null.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="type">The type of the service required.</param>
        /// <param name="otherwise">If the service can not ve resolved, an optional instance instead.</param>
        /// <returns>An instance of <see cref="type"/>.</returns>
        internal static object GetServiceIfAvailable(this IServiceProvider serviceProvider, Type type, Func<object> otherwise)
        {
            return serviceProvider?.GetService(type) ?? otherwise();
        }

        /// <summary>
        /// Gets the service object of the specified type, or provided instance if the service can not be resolved,
        /// or the service provider is null.
        /// </summary>
        /// <typeparam name="T">The type of service required.</typeparam>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="otherwise">If the service can not ve resolved, an optional instance instead.</param>
        /// <returns>An instance of <see cref="T"/></returns>
        internal static T GetServiceIfAvailable<T>(this IServiceProvider serviceProvider, Func<T> otherwise) where T: class
        {
            return serviceProvider?.GetService<T>() ?? otherwise();
        }
    }
}