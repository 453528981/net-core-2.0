using System;
using System.Collections.Generic;

namespace Ayatta.Extension
{
    public static partial class Extension
    {
        /// <summary>
        /// Execute <paramref name="action"/> for each element of <paramref name="sequence"/>.
        /// </summary>
        /// <typeparam name="T">Type of items in <paramref name="sequence"/>.</typeparam>
        /// <param name="sequence">Sequence of items to act on.</param>
        /// <param name="action">Action to invoke for each item.</param>
        //[DebuggerStepThrough]
        public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            sequence = sequence ?? new T[0];
            foreach (var item in sequence)
            {
                action(item);
            }
        }

        public static IEnumerable<T> GetDescendants<T>(this T root, Func<T, IEnumerable<T>> childSelector,
                                                       Predicate<T> filter)
        {
            foreach (var t in childSelector(root))
            {
                if (filter == null || filter(t))
                    yield return t;
                foreach (var child in GetDescendants(t, childSelector, filter))
                    yield return child;
            }
        }


        public static TSource Aggregate<TSource>(this IEnumerable<TSource> source,
                                                 Func<TSource, TSource, int, TSource> func)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            var index = 0;
            using (var enumerator = source.GetEnumerator())
            {
                enumerator.MoveNext();
                index++;
                var current = enumerator.Current;
                while (enumerator.MoveNext())
                    current = func(current, enumerator.Current, index++);
                return current;
            }
        }

        /// <summary>
        /// Convenient replacement for a range 'for' loop. e.g. return an array of int from 10 to 20:
        /// int[] tenToTwenty = 10.to(20).ToArray();
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>

        public static IEnumerable<int> To(this int from, int to)
        {
            for (var i = from; i <= to; i++)
            {
                yield return i;
            }
        }


        public static IEnumerable<T> AtOddPositions<T>(this IEnumerable<T> list)
        {
            var odd = false; // 0th position is even
            foreach (var item in list)
            {
                odd = !odd;
                if (odd)
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<T> AtEvenPositions<T>(this IEnumerable<T> list)
        {
            var even = true; // 0th position is even
            foreach (var item in list)
            {
                even = !even;
                if (even)
                {
                    yield return item;
                }
            }
        }
    }
}