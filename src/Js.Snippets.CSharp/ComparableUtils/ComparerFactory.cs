namespace Js.Snippets.CSharp.ComparableUtils
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A factory method for creating <see cref="IComparer{T}"/> instances from a lambda expression
    /// </summary>
    public static class ComparerFactory
    {
        /// <summary>
        /// A factory method for creating <see cref="IComparer{T}"/> instances from a lambda expression
        /// specifying the less-than relation.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="lessThan">A lambda expression specifying the less-than relation.</param>
        /// <returns>An <see cref="IComparer{T}"/> instance.</returns>
        public static IComparer<T> Create<T>(Func<T, T, bool> lessThan)
        {
            return new Comparer<T>(lessThan);
        }

        private class Comparer<T> : IComparer<T>
        {
            private readonly Func<T, T, bool> lessThan;

            public Comparer(Func<T, T, bool> lessThan)
            {
                this.lessThan = lessThan;
            }

            public int Compare(T x, T y)
            {
                if (lessThan(x, y) && !lessThan(y, x))
                {
                    return -1;
                }
                else if (lessThan(y, x) && !lessThan(x, y))
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
