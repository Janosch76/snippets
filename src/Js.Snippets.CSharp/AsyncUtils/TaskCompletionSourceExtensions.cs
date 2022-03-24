using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Js.Snippets.CSharp.AsyncUtils
{
    /// <summary>
    /// Extension methods for the <see cref="TaskCompletionSource{TResult}"/> class.
    /// </summary>
    public static class TaskCompletionSourceExtensions
    {
        /// <summary>
        /// Attempts to complete the underlying <see cref="Task{T}"/>,
        /// propagating the completion status of the specified 
        /// <see cref="AsyncCompletedEventArgs"/> event args.
        /// </summary>
        /// <typeparam name="T">The task result type.</typeparam>
        /// <param name="tcs">The task completion source.</param>
        /// <param name="args">The event args from which to set the completion status.</param>
        /// <param name="getResult">A method to provide the result in case of successful completion.</param>
        /// <returns>True, if the operation was successful; false, if the operation was unsuccessful or the object has already been disposed.</returns>
        /// <remarks>
        /// Helps interoperate between TAP (Task-based Asynchronous Programming, async), and 
        /// EAP (Event-based Asynchronous Programming, *Completed).
        /// See <a href="https://github.com/StephenCleary/AsyncEx/wiki/TaskCompletionSourceExtensions"/> for more.
        /// </remarks>
        public static bool TryCompleteFromEventArgs<T>(this TaskCompletionSource<T> tcs, AsyncCompletedEventArgs args, Func<T> getResult)
        {
            if (args.Cancelled)
            {
                return tcs.TrySetCanceled();
            }
            else if (args.Error != null)
            {
                return tcs.TrySetException(args.Error);
            }
            else
            {
                return tcs.TrySetResult(getResult());
            }
        }
    }
}
