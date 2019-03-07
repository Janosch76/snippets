namespace Js.Snippets.CSharp.AsyncUtils
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Async-friendly version of <see cref="Lazy{T}"/>
    /// </summary>
    /// <typeparam name="T">The type that is being initialized.</typeparam>
    /// <see cref="https://cpratt.co/thread-safe-strongly-typed-memory-caching-c-sharp/"/>
    public class AsyncLazy<T> : Lazy<Task<T>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncLazy{T}"/> class.
        /// </summary>
        /// <param name="valueFactory">The value factory method.</param>
        public AsyncLazy(Func<T> valueFactory)
            : base(() => Task.Factory.StartNew(valueFactory))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncLazy{T}"/> class.
        /// </summary>
        /// <param name="taskFactory">The value factory method></param>
        public AsyncLazy(Func<Task<T>> taskFactory)
            : base(() => Task.Factory.StartNew(() => taskFactory()).Unwrap())
        {
        }
    }
}
