using MoreLinq;

namespace RevitAddIn1.Utils
{
    public static  class MoreEnumerable2
    {

        public static IEnumerable<TSource> DistinctBy2<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)

        {
            return  MoreEnumerable.DistinctBy(source, keySelector, null);
        }
     
    }
}