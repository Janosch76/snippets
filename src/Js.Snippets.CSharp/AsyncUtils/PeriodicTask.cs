namespace Js.Snippets.CSharp.AsyncUtils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A task that runs periodically with a specified delay interval.
    /// </summary>
    public static class PeriodicTask
    {
        /// <summary>
        /// Repeats the specified action a given number of times, with a specified delay interval.
        /// </summary>
        /// <param name="repeats">The repeats.</param>
        /// <param name="action">The action.</param>
        /// <param name="delay">The delay.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The repeatedly executing task.</returns>
        public static async Task Repeat(int repeats, Action action, TimeSpan delay, CancellationToken cancellationToken = default(CancellationToken))
        {
            for (int i = 0; i < repeats; i++)
            {
                await ExecuteAfterDelay(action, delay, cancellationToken);
            }
        }

        /// <summary>
        /// Repeats the specified action a given number of times, with a specified delay interval.
        /// </summary>
        /// <param name="repeats">The repeats.</param>
        /// <param name="task">The task.</param>
        /// <param name="delay">The delay.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The repeatedly executing task.</returns>
        public static async Task Repeat(int repeats, Func<Task> task, TimeSpan delay, CancellationToken cancellationToken = default(CancellationToken))
        {
            await PeriodicTask.Repeat(repeats, async (CancellationToken ct) => await task(), delay, cancellationToken);
        }

        /// <summary>
        /// Repeats the specified action a given number of times, with a specified delay interval.
        /// </summary>
        /// <param name="repeats">The repeats.</param>
        /// <param name="task">The task.</param>
        /// <param name="delay">The delay.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The repeatedly executing task.</returns>
        public static async Task Repeat(int repeats, Func<CancellationToken, Task> task, TimeSpan delay, CancellationToken cancellationToken = default(CancellationToken))
        {
            for (int i = 0; i < repeats; i++)
            {
                await ExecuteAfterDelay(task, delay, cancellationToken);
            }
        }

        /// <summary>
        /// Runs the specified action periodically, with a specified delay interval.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="delay">The delay.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The periodically executing task.</returns>
        public static async Task Run(Action action, TimeSpan delay, CancellationToken cancellationToken = default(CancellationToken))
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await ExecuteAfterDelay(action, delay, cancellationToken);
            }
        }

        /// <summary>
        /// Runs the specified task periodically, with a specified delay interval.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="delay">The delay.</param>
        /// <returns>The periodically executing task.</returns>
        public static async Task Run(Func<Task> task, TimeSpan delay)
        {
            await PeriodicTask.Run(async (CancellationToken ct) => await task(), delay);
        }

        /// <summary>
        /// Runs the specified task periodically, with a specified delay interval.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="delay">The delay.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The periodically executing task.</returns>
        public static async Task Run(Func<CancellationToken, Task> task, TimeSpan delay, CancellationToken cancellationToken = default(CancellationToken))
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await ExecuteAfterDelay(task, delay, cancellationToken);
            }
        }

        private static async Task ExecuteAfterDelay(Action action, TimeSpan delay, CancellationToken cancellationToken)
        {
            await Task.Delay(delay, cancellationToken);

            if (!cancellationToken.IsCancellationRequested)
            {
                action();
            }
        }

        private static async Task ExecuteAfterDelay(Func<CancellationToken, Task> task, TimeSpan delay, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Task.Delay(delay, cancellationToken);

            if (!cancellationToken.IsCancellationRequested)
            {
                await task(cancellationToken);
            }
        }
    }
}
