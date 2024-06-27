using Mapster;
using System.Linq;

namespace InHues.Application.Common.Extensions
{
    public static class MapsterExtension
    {
        public static T CustomAdapt<T>(this object source) => source is not null ? source.Adapt<T>() : default(T);
        public static IQueryable<TDestination> CustomProjectToType<TSource,TDestination>(this IQueryable<TSource> source) => source.Any() ? source.ProjectToType<TDestination>() : null;
    }
}
