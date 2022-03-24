using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Js.Snippets.CSharp.AsyncUtils
{
    /// <summary>
    /// Extension methods for async event handlers.
    /// </summary>
    public static class AsyncEventHandlerExtensions
    {
        /// <summary>
        /// Invokes asynchronous event handlers, returning a task that completes 
        /// when all event handlers have been invoked sequentially: 
        /// Each handler is fully executed (including continuations)
        /// before the next handler in the list is invoked.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of event data.</typeparam>
        /// <param name="handler">The delegate to invoke.</param>
        /// <param name="sender">The event sender</param>
        /// <param name="args">The event data</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task that signals completion of the operation.</returns>
        public static async Task InvokeAsync<TEventArgs>(this AsyncEventHandler<TEventArgs> handler, object sender, TEventArgs args, CancellationToken cancellationToken = default)
            where TEventArgs : EventArgs
        {
            if (handler == null)
            {
                return;
            }

            var delegates = handler
                .GetInvocationList()
                .Cast<AsyncEventHandler<TEventArgs>>();
            foreach (var @delegate in delegates)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await @delegate(sender, args, cancellationToken);
            }
        }

        /// <summary>
        /// Invokes asynchronous event handlers, returning a task that completes 
        /// when all event handlers have been invoked sequentially: 
        /// Each handler is fully executed (including continuations)
        /// before the next handler in the list is invoked.
        /// </summary>
        /// <param name="handler">The delegate to invoke.</param>
        /// <param name="sender">The event sender</param>
        /// <param name="args">The event data</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task that signals completion of the operation.</returns>
        public static async Task InvokeAsync(this AsyncEventHandler handler, object sender, EventArgs args, CancellationToken cancellationToken = default)
        {
            if (handler == null)
            {
                return;
            }

            var delegates = handler
                .GetInvocationList()
                .Cast<AsyncEventHandler>();
            foreach (var @delegate in delegates)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await @delegate(sender, args, cancellationToken);
            }
        }
    }
}
