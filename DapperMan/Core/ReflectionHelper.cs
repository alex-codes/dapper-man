using DapperMan.Core.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;

namespace DapperMan.Core
{
    public static class ReflectionHelper
    {
        public static string GetIdentityField<T>(ObjectCache propertyCache) where T : class
        {
            Type type = typeof(T);
            string cacheKey = $"{type.FullName}_key";
            string metaPropsCacheKey = $"{type.FullName}_meta";
            string propsCacheKey = $"{type.FullName}_props";

            PropertyInfo[] metadataProps = null;
            PropertyInfo[] props = null;
            MetadataTypeAttribute metaType = null;

            if (propertyCache != null)
            {
                if (propertyCache.Contains(cacheKey))
                {
                    return propertyCache.Get(cacheKey) as string;
                }
                else if (propertyCache.Contains(metaPropsCacheKey) && propertyCache.Contains(propsCacheKey))
                {
                    metadataProps = propertyCache.Get(metaPropsCacheKey) as PropertyInfo[];
                    props = propertyCache.Get(propsCacheKey) as PropertyInfo[];
                }
            }

            var policy = new CacheItemPolicy();
            policy.SlidingExpiration = TimeSpan.FromMinutes(15);

            if (metadataProps == null || props == null)
            {
                metaType = type.GetCustomAttributes(typeof(MetadataTypeAttribute), false)
                    .OfType<MetadataTypeAttribute>()
                    .FirstOrDefault();

                if (metaType != null)
                {
                    metadataProps = metaType.MetadataClassType.GetProperties();
                }

                props = type.GetProperties();

                if (propertyCache != null)
                {
                    propertyCache.Set(metaPropsCacheKey, metadataProps, policy);
                    propertyCache.Set(propsCacheKey, props, policy);
                }
            }

            PropertyInfo key = null;
            string keyName = null;

            if (metaType != null)
            {
                key = props.FirstOrDefault(p => p.CustomAttributes.Any(ca => ca.AttributeType == typeof(IdentityAttribute)));

                if (key == null)
                {
                    key = props.FirstOrDefault(p => p.CustomAttributes.Any(ca => ca.AttributeType == typeof(IdentityAttribute)));

                    if (key != null)
                    {
                        keyName = key.Name;
                    }
                }
                else
                {
                    keyName = key.Name;
                }
            }

            if (key == null)
            {
                key = props.FirstOrDefault(p => p.CustomAttributes.Any(ca => ca.AttributeType == typeof(IdentityAttribute)));

                if (key == null)
                {
                    key = props.FirstOrDefault(p => p.CustomAttributes.Any(ca => ca.AttributeType == typeof(IdentityAttribute)));

                    if (key != null)
                    {
                        keyName = key.Name;
                    }
                }
                else
                {
                    keyName = key.Name;
                }
            }

            if (propertyCache != null)
            {
                propertyCache.Set(cacheKey, keyName, policy);
            }

            return null;
        }

        public static bool IncludeProperty(PropertyInfo[] metadataProps, PropertyInfo prop, Type[] ignoreAttributes)
        {
            var mdp = metadataProps.FirstOrDefault(p => p.Name == prop.Name);

            bool includeMeta = mdp == null
                || !mdp.CustomAttributes.Any(ca => IsAttributeIgnored(ca, ignoreAttributes));

            bool includeAttr = !prop.CustomAttributes.Any(ca => IsAttributeIgnored(ca, ignoreAttributes));

            return includeMeta && includeAttr;
        }

        private static bool IsAttributeIgnored(CustomAttributeData attrib, Type[] ignoreAttributes)
        {
            return ignoreAttributes.Any(attr => attrib.AttributeType == attr);
        }

        // http://stackoverflow.com/a/6949037/1087945 and http://stackoverflow.com/a/6201859/1087945 for retrieving the metadata attributes
        public static string[] ReflectProperties<T>(ObjectCache propertyCache, Type[] ignoreAttributes) where T : class
        {
            Type type = typeof(T);
            string cacheKey = $"{type.FullName}_propNames";

            if (propertyCache != null)
            {
                if (propertyCache.Contains(cacheKey))
                {
                    return propertyCache.Get(cacheKey) as string[];
                }
            }

            var props = type.GetProperties();
            string metaPropsCacheKey = $"{type.FullName}_meta";
            string propsCacheKey = $"{type.FullName}_props";

            var metaType = type.GetCustomAttributes(typeof(MetadataTypeAttribute), false)
                .OfType<MetadataTypeAttribute>()
                .FirstOrDefault();

            Type metadataType = null;
            var metadataProps = (metadataType != null ? metadataType.GetProperties() : new PropertyInfo[0]);

            if (metaType != null)
            {
                metadataType = metaType.MetadataClassType;
            }

            var properties = new List<string>();

            foreach (var prop in props)
            {
                if ((prop.PropertyType.IsValueType || prop.PropertyType == typeof(string)))
                {
                    if (IncludeProperty(metadataProps, prop, ignoreAttributes))
                    {
                        properties.Add(prop.Name);
                    }
                }
            }

            if (propertyCache != null)
            {
                var policy = new CacheItemPolicy();
                policy.SlidingExpiration = TimeSpan.FromMinutes(15);

                propertyCache.Set(cacheKey, properties.ToArray(), policy);
                propertyCache.Set(metaPropsCacheKey, metadataProps, policy);
                propertyCache.Set(propsCacheKey, props, policy);
            }

            return properties.ToArray();
        }
    }
}
