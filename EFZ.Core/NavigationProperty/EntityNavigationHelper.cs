using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using EFZ.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFZ.Core.NavigationProperty
{
    public class EntityNavigationHelper
    {
        public static Func<IQueryable<T>, IQueryable<T>> GetNavigations<T>() where T : class
        {
            var type = typeof(T);
            var navigationProperties = new List<string>();

            var listIncluded = new List<string>();

            GetNavigationProperties(type, type, string.Empty, navigationProperties, listIncluded);

            Func<IQueryable<T>, IQueryable<T>> includes = (query => {
                return navigationProperties.Aggregate(query, (current, inc) => current.Include(inc));
            });

            return includes;
        }

        private static void GetNavigationProperties(Type baseType, Type type, string parentPropertyName, IList<string> accumulator, IList<string> listIncluded)
        {
            var properties = type.GetProperties();
            var navigationPropertyInfoList = properties.Where(prop => prop.IsDefined(typeof(NavigationPropertyAttribute)));

            foreach (var prop in navigationPropertyInfoList)
            {
                var propertyType = prop.PropertyType;
                var elementType = propertyType.GetTypeInfo().IsGenericType ? propertyType.GetGenericArguments()[0] : propertyType;

                if (!listIncluded.Any(item => item.Equals(prop.Name)))
                    listIncluded.Add(prop.Name);
                else
                {
                    continue;
                }
                var propertyName =
                    $"{parentPropertyName}{(string.IsNullOrEmpty(parentPropertyName) ? string.Empty : ".")}{prop.Name}";
                accumulator.Add(propertyName);

                
                var isJsonIgnored = prop.IsDefined(typeof(JsonIgnoreAttribute));
                if (!isJsonIgnored && elementType.Name != baseType.Name)
                {
                    GetNavigationProperties(baseType, elementType, propertyName, accumulator, listIncluded);
                }
            }
        }
    }
}
