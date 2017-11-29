using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Paradigm.ORM.Data.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="Type"/> class.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Gets the property information from an expression of the type  (x => x.ProperyName).
        /// </summary>
        /// <typeparam name="TType">A .NET type.</typeparam>
        /// <typeparam name="TProperty">The type of a property inside <see cref="TType"/>.</typeparam>
        /// <param name="expression">A expression returning a property of type <see cref="TProperty"/></param>
        /// <returns>A <see cref="PropertyInfo"/> instance of the referenced property.</returns>
        /// <exception cref="System.ArgumentException">Expression is not a Property. - expression</exception>
        public static PropertyInfo GetPropertyInfo<TType, TProperty>(Expression<Func<TType, TProperty>> expression)
        {
            var member = expression.Body as MemberExpression;

            if (member?.Member is PropertyInfo)
                return (PropertyInfo) member.Member;

            throw new ArgumentException("Expression is not a Property.", nameof(expression));
        }

        /// <summary>
        /// Gets the property information from an expression of the type  (x => x.ProperyName).
        /// </summary>
        /// <typeparam name="TType">A .NET type.</typeparam>
        /// <param name="expression">A expression returning a property.</param>
        /// <returns>A <see cref="PropertyInfo"/> instance of the referenced property.</returns>
        /// <exception cref="System.ArgumentException">Expression is not a Property. - expression</exception>
        public static PropertyInfo GetPropertyInfo<TType>(Expression<Func<TType, object>> expression)
        {
            var unary = expression.Body as UnaryExpression;
            var member = unary?.Operand as MemberExpression;

            if (member?.Member is PropertyInfo)
                return (PropertyInfo)member.Member;

            throw new ArgumentException("Expression is not a Property.", nameof(expression));
        }

        /// <summary>
        /// Gets the default value of a given type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Default value.</returns>
        public static object GetDefaultValue(this Type type)
        {
            return type.GetTypeInfo().GetDefaultValue();
        }

        /// <summary>
        /// Gets a list of all the base types and interfaces.
        /// </summary>
        /// <param name="type">Type to extract parents.</param>
        /// <returns>A lis</returns>
        public static HashSet<Type> GetParentTypes(this Type type)
        {
            var typeInfo = type?.GetTypeInfo();
            var types = new HashSet<Type>();

            if ((type == null) || (typeInfo.BaseType == null))
            {
                return types;
            }

            foreach (var interfaceType in typeInfo.GetInterfaces())
            {
                if (!types.Contains(interfaceType))
                {
                    types.Add(interfaceType);
                }

                var interfaces = interfaceType.GetParentTypes();

                foreach (var parentInterfaceType in interfaces)
                {
                    if (!types.Contains(parentInterfaceType))
                    {
                        types.Add(parentInterfaceType);
                    }
                }
            }

            var baseType = typeInfo.BaseType;

            while (baseType != null)
            {
                if (!types.Contains(baseType))
                {
                    types.Add(baseType);
                }

                var baseTypeInfo = baseType.GetTypeInfo();
                var baseTypes = baseType.GetParentTypes();

                foreach(var parentBaseType in baseTypes)
                {
                    if (!types.Contains(parentBaseType))
                    {
                        types.Add(parentBaseType);
                    }
                }

                baseType = baseTypeInfo.BaseType;
            }

            return types;
        }
    }
}