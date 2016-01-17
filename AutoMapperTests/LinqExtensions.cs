using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoMapperTests
{
    public static class LinqExtensions
    {
        public static IEnumerable<TResult> LeftOuterJoin<TSource, TInner, TKey, TResult>(this IEnumerable<TSource> source, IEnumerable<TInner> other, Func<TSource, TKey> func, Func<TInner, TKey> innerkey, Func<TSource, TInner, TResult> res)
        {
            return from f in source
                   join b in other on func.Invoke(f) equals innerkey.Invoke(b) into g
                   from result in g.DefaultIfEmpty()
                   select res.Invoke(f, result);
        }
    }
}