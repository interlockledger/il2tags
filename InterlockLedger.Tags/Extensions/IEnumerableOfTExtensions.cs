/******************************************************************************************************************************
 *
 *      Copyright (c) 2017-2019 InterlockLedger Network
 *
 ******************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;

namespace InterlockLedger.Tags
{
    public static class IEnumerableOfTExtensions
    {
        public static bool AnyWithNoNulls<T>(this IEnumerable<T> items) => items.SafeAny() && items.NoNulls();

        public static IEnumerable<T> AppendedOf<T>(this T item, IEnumerable<T> remainingItems) => InnerAppend(item, remainingItems);

        public static IEnumerable<T> AppendedOf<T>(this T item, params T[] remainingItems) => InnerAppend(item, remainingItems);

        public static bool EqualTo<T>(this IEnumerable<T> first, IEnumerable<T> second) {
            if (first.SafeCount() != second.SafeCount())
                return false;
            return first?.SequenceEqual(second) ?? true;
        }

        public static IEnumerable<T> IfAnyDo<T>(this IEnumerable<T> values, Action action) {
            if (values.SafeAny())
                action();
            return values;
        }

        public static string JoinedBy<T>(this IEnumerable<T> list, string joiner) => list == null ? string.Empty : string.Join(joiner, list);

        public static bool None<T>(this IEnumerable<T> items) => !items.SafeAny();

        public static bool None<T>(this IEnumerable<T> items, Func<T, bool> predicate) => !items.SafeAny(predicate);

        public static bool NoNulls<T>(this IEnumerable<T> items) => items.None(item => item is null);

        public static bool SafeAny<T>(this IEnumerable<T> values) => values?.Any() ?? false;

        public static bool SafeAny<T>(this IEnumerable<T> items, Func<T, bool> predicate) => items?.Any(predicate) == true;

        public static IEnumerable<T> SafeConcat<T>(this IEnumerable<T> items, IEnumerable<T> remainingItems)
            => InnerConcat(items ?? Enumerable.Empty<T>(), remainingItems);

        public static int SafeCount<T>(this IEnumerable<T> values) => values?.Count() ?? -1;

        public static IEnumerable<TResult> SelectSkippingNulls<TSource, TResult>(this IEnumerable<TSource> values, Func<TSource, TResult> selector) where TResult : class
            => EmptyIfNull(values?.Select(selector).SkipNulls());

        public static IEnumerable<T> SkipNulls<T>(this IEnumerable<T> values) where T : class => EmptyIfNull(values?.Where(item => item != null));

        public static string WithCommas<T>(this IEnumerable<T> list, bool noSpaces = false) => JoinedBy(list, noSpaces ? "," : ", ");

        public static IEnumerable<T> WithDefault<T>(this IEnumerable<T> values, Func<IEnumerable<T>> alternativeValues)
            => values.SafeAny() ? values : EmptyIfNull(alternativeValues?.Invoke());

        public static IEnumerable<T> WithDefault<T>(this IEnumerable<T> values) => EmptyIfNull(values);

        public static IEnumerable<T> WithDefault<T>(this IEnumerable<T> values, params T[] alternativeValues)
            => WithDefault(values, (IEnumerable<T>)alternativeValues);

        public static IEnumerable<T> WithDefault<T>(this IEnumerable<T> values, IEnumerable<T> alternativeValues)
            => values.SafeAny() ? values : EmptyIfNull(alternativeValues);

        private static IEnumerable<T> EmptyIfNull<T>(IEnumerable<T> values) => values ?? Enumerable.Empty<T>();

        private static IEnumerable<T> InnerAppend<T>(T item, IEnumerable<T> remainingItems)
            => new T[] { item }.SafeConcat(remainingItems);

        private static IEnumerable<T> InnerConcat<T>(IEnumerable<T> items, IEnumerable<T> remainingItems)
            => remainingItems.SafeAny() ? items.Concat(remainingItems) : items;
    }
}