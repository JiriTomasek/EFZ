using System;
using AutoMapper;

namespace EFZ.Core.Mapping
{
    public static class MappingUtils
    {
        public static TDestination MapToNew<TSource, TDestination>(TSource source,
            Action<TSource, TDestination> beforeMap = null,
            Action<TSource, TDestination> afterMap = null)
            where TSource : class
            where TDestination : class, new()
        {
            return MapToExisting(source, new TDestination(), beforeMap, afterMap);
        }

        public static TDestination MapToExisting<TSource, TDestination>(TSource source,
            TDestination destination,
            Action<TSource, TDestination> beforeMap = null,
            Action<TSource, TDestination> afterMap = null)
            where TSource : class
            where TDestination : class
        {
            beforeMap ??= delegate { };
            afterMap ??= delegate { };

            var config = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDestination>().BeforeMap(beforeMap).AfterMap(afterMap));
            var mapper = config.CreateMapper();
            return mapper.Map(source, destination);
        }
    }
}
