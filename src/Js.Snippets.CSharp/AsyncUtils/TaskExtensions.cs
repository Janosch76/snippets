namespace Js.Snippets.CSharp.AsyncUtils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary> 
        /// Executes the given action on each of the tasks in turn. The action is passed the result of each task. 
        /// </summary> 
        /// <typeparam name="T">The task result type.</typeparam>
        /// <param name="tasks">The tasks.</param>
        /// <param name="action">The action.</param>
        /// <returns>A task that asynchronously applies the given action on each task result.</returns>
        public static async Task ForEach<T>(this IEnumerable<Task<T>> tasks, Action<T> action)
        {
            foreach (var task in tasks)
            {
                T value = await task;
                action(value);
            }
        }

        /// <summary> 
        /// Returns a sequence of tasks which will be observed to complete with the same set 
        /// of results as the given input tasks, but in the order in which the original tasks complete. 
        /// </summary>
        /// <typeparam name="T">The task result type.</typeparam>
        /// <param name="inputTasks">The tasks.</param>
        /// <returns>An enumeration of the completed tasks, in order of completion.</returns>
        /// <remarks>See <seealso cref="https://blogs.msmvps.com/jonskeet/2012/01/16/eduasync-part-19-ordering-by-completion-ahead-of-time/"/>.</remarks>
        public static IEnumerable<Task<T>> OrderByCompletion<T>(this IEnumerable<Task<T>> inputTasks)
        {
            var inputTaskList = inputTasks.ToList();

            var completionSources = new TaskCompletionSource<T>[inputTaskList.Count];
            for (int i = 0; i < inputTaskList.Count; i++)
            {
                completionSources[i] = new TaskCompletionSource<T>();
            }

            int prevIndex = -1;
            Action<Task<T>> continuation = completedTask =>
            {
                int index = Interlocked.Increment(ref prevIndex);
                var source = completionSources[index];
                PropagateResult(completedTask, source);
            };
            foreach (var inputTask in inputTaskList)
            {
                inputTask.ContinueWith(continuation, TaskContinuationOptions.ExecuteSynchronously);
            }

            return completionSources.Select(source => source.Task);
        }

        /// <summary> 
        /// Propagates the status of the given task (which must be completed) to a task completion source 
        /// (which should not be). 
        /// </summary> 
        /// <typeparam name="T">The task result type.</typeparam>
        /// <param name="completedTask">The completed task.</param>
        /// <param name="completionSource">The task completion source.</param>
        public static void PropagateResult<T>(this Task<T> completedTask, TaskCompletionSource<T> completionSource)
        {
            switch (completedTask.Status)
            {
                case TaskStatus.Canceled:
                    completionSource.TrySetCanceled();
                    break;
                case TaskStatus.Faulted:
                    completionSource.TrySetException(completedTask.Exception.InnerExceptions);
                    break;
                case TaskStatus.RanToCompletion:
                    completionSource.TrySetResult(completedTask.Result);
                    break;
                default:
                    throw new ArgumentException("Task was not completed");
            }
        }
    }
}
