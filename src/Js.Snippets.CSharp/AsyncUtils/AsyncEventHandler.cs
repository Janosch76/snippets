using System;
using System.Threading;
using System.Threading.Tasks;

namespace Js.Snippets.CSharp.AsyncUtils
{
    /// <summary>
    /// Signature for an async-friendly, awaitable event handler.
    /// </summary>
    /// <typeparam name="TEventArgs">The type of event data.</typeparam>
    /// <param name="sender">The event sender</param>
    /// <param name="args">The event data</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task that signals completion of the operation.</returns>
    public delegate Task AsyncEventHandler<in TEventArgs>(object sender, TEventArgs args, CancellationToken cancellationToken)
        where TEventArgs : EventArgs;

    /// <summary>
    /// Signature for an async-friendly, awaitable event handler.
    /// </summary>
    /// <param name="sender">The event sender</param>
    /// <param name="args">The event data</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task that signals completion of the operation.</returns>
    public delegate Task AsyncEventHandler(object sender, EventArgs args, CancellationToken cancellationToken);
}