using DapperMan.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DapperMan.Core
{
    /// <summary>
    /// Contains helper methods for reflecting metadata.
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// Identifies the field decorated with an <see cref="IdentityAttribute"/>
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="propertyCache">An object used for caching information about the typed object.</param>
        /// <returns>
        /// The name of the property decorated with an <see cref="IdentityAttribute"/>, or null.
        /// </returns>
        public static string GetIdentityField<T>(PropertyCache propertyCache) where T : class
        {
            Type type = typeof(T);
            string cacheKey = $"{type.FullName}_key";
            string propsCacheKey = $"{type.FullName}_props";

            if (propertyCache?.Cache != null)
            {
                if (propertyCache.Cache.Contains(cacheKey))
                {
                    return propertyCache.Cache.Get(cacheKey) as string;
                }
            }

            string keyName = null;

            PropertyInfo[] props = type.GetProperties();
            PropertyInfo key = props.FirstOrDefault(p => p.CustomAttributes.Any(ca => ca.AttributeType == typeof(IdentityAttribute)));

            if (key != null)
            {
                keyName = key.Name;
            }

            if (propertyCache != null)
            {
                propertyCache.Cache.Set(cacheKey, keyName, propertyCache.Policy);
            }

            return keyName;
        }

        /// <summary>
        /// Determines whether or not to include a property in the query.
        /// </summary>
        /// <param name="prop">An object property.</param>
        /// <param name="ignoreAttributes">A list of attribute types. 
        /// If the property is decorated with one of the supplied attributes, 
        /// the property will not be included in the query.
        /// </param>
        /// <returns>False if the property is decorated with one of the ignored attributes, otherwise true.</returns>
        public static bool IncludeProperty(PropertyInfo prop, Type[] ignoreAttributes)
        {
            return !prop.CustomAttributes.Any(ca => IsAttributeIgnored(ca, ignoreAttributes));
        }

        /// <summary>
        /// Determines if the custom attribute is one of the ignored attribute types.
        /// </summary>
        /// <param name="attrib">A custom attribute.</param>
        /// <param name="ignoreAttributes">A list of attribute types. 
        /// If the property is decorated with one of the supplied attributes, 
        /// the property will not be included in the query.
        /// </param>
        /// <returns>False if the property is decorated with one of the ignored attributes, otherwise true.</returns>
        private static bool IsAttributeIgnored(CustomAttributeData attrib, Type[] ignoreAttributes)
        {
            return ignoreAttributes.Any(attr => attrib.AttributeType == attr);
        }

        // http://stackoverflow.com/a/6949037/1087945 and http://stackoverflow.com/a/6201859/1087945 for retrieving the metadata attributes
        /// <summary>
        /// Reflects the properties of a given object and returns a list of property names.
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="propertyCache">An object used for caching information about the typed object.</param>
        /// <param name="ignoreAttributes">A list of attribute types. 
        /// If the property is decorated with one of the supplied attributes, 
        /// the property will not be included in the query.
        /// </param>
        /// <returns>A list of property names to include in the query.</returns>
        public static string[] ReflectProperties<T>(PropertyCache propertyCache, Type[] ignoreAttributes) where T : class
        {
            Type type = typeof(T);
            string cacheKey = $"{type.FullName}_propNames";

            if (propertyCache != null)
            {
                if (propertyCache.Cache.Contains(cacheKey))
                {
                    return propertyCache.Cache.Get(cacheKey) as string[];
                }
            }

            var props = type.GetProperties();
            string propsCacheKey = $"{type.FullName}_props";

            var properties = new List<string>();

            foreach (var prop in props)
            {
                if ((prop.PropertyType.IsValueType || prop.PropertyType == typeof(string)))
                {
                    if (IncludeProperty(prop, ignoreAttributes))
                    {
                        properties.Add(prop.Name);
                    }
                }
            }

            if (propertyCache != null)
            {
                propertyCache.Cache.Set(cacheKey, properties.ToArray(), propertyCache.Policy);
                propertyCache.Cache.Set(propsCacheKey, props, propertyCache.Policy);
            }

            return properties.ToArray();
        }
    }
}
