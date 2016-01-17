using System;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace AutoMapperTests
{
    public static class ModelsExtensions
    {
        public static IQueryable<TOut> Map<TIn, TOut>(this IQueryable<TIn> source)
            where TIn : class
            where TOut : class
        {
            return source.Project().To<TOut>();
        }

        public static TOut Map<TIn, TOut>(this TIn source, TOut dest = null)
            where TIn : class
            where TOut : class
        {
            return dest == null
                ? (source == null ? null : Mapper.Map<TIn, TOut>(source))
                : (source == null ? dest : Mapper.Map(source, dest));
        }

        public static TOut MapFrom<TOut, TIn>(this TOut dest, TIn source)
            where TOut : class
            where TIn : class
        {
            if (dest == null) throw new NotSupportedException();

            return (source == null ? dest : Mapper.Map(source, dest));
        }
    }
}