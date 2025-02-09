namespace PgcSharedLib.Helpers
{
    /// <summary>
    /// Retry helper for calling the methods.
    /// </summary>
    public static class RetryHelper
    {
        #region Fields

        // Milliseconds intervals
        private static readonly ushort[] DefaultSpans = { 500, 400, 200 };

        #endregion

        #region Public Methods

        /// <summary>
        /// Invokes an action with retry.
        /// If an exception that occured is a transient error or the invoker wants to retry, retry several times.
        /// </summary>
        /// <param name="onAction">The invoking action.</param>
        /// <param name="transientExceptionFilter">
        /// The check logic whether the exception thrown is a transient exception.
        /// Retry invoking the action, if the check result is <c>true</c>. Otherwise; throws the exception.
        /// </param>
        /// <param name="retrySpans">The time spans to retry. The unit of them is milliseconds.</param>
        /// <exception cref="ArgumentNullException"><paramref name="onAction"/> is <c>null</c>.</exception>
        public static void InvokeWithRetry(this Action onAction, Predicate<Exception> transientExceptionFilter, params ushort[] retrySpans)
        {
            if (transientExceptionFilter is null)
            {
                onAction.InvokeWithRetry(retrySpans);
                return;
            }

            if (onAction is null)
            {
                throw new ArgumentNullException(nameof(onAction));
            }

            var intervals = GetIntervalsSafe(retrySpans);
            foreach (var interval in intervals)
            {
                try
                {
                    onAction.Invoke();
                    return;
                }
                catch (Exception ex)
                {
                    if (!transientExceptionFilter(ex))
                    {
                        // If this isn't a transient error or we shouldn't retry,
                        // rethrow the exception.
                        throw;
                    }

                    Task.Delay(interval).Wait();
                }
            }

            onAction.Invoke();
        }

        /// <summary>
        /// Invokes an action with retry. If an exception is occured, retry several times.
        /// </summary>
        /// <param name="onAction">The invoking action.</param>
        /// <param name="retrySpans">The time spans to retry. The unit of them is milliseconds.</param>
        /// <exception cref="ArgumentNullException"><paramref name="onAction"/> is <c>null</c>.</exception>
        public static void InvokeWithRetry(this Action onAction, params ushort[] retrySpans)
        {
            if (onAction is null)
            {
                throw new ArgumentNullException(nameof(onAction));
            }

            var intervals = GetIntervalsSafe(retrySpans);
            foreach (var interval in intervals)
            {
                try
                {
                    onAction.Invoke();
                    return;
                }
                catch (Exception)
                {
                    Task.Delay(interval).Wait();
                }
            }

            onAction.Invoke();
        }

        /// <summary>
        /// Invokes an action with retry.
        /// If an exception that occured is a transient error or the invoker wants to retry, retry several times.
        /// </summary>
        /// <typeparam name="TResult">The type of invoking the method result.</typeparam>
        /// <param name="execute">The invoking action.</param>
        /// <param name="transientExceptionFilter">
        /// The check logic whether the exception thrown is a transient exception.
        /// Retry invoking the action, if the check result is <c>true</c>. Otherwise; throws the exception.
        /// </param>
        /// <param name="retrySpans">The time spans to retry. The unit of them is milliseconds.</param>
        /// <returns>An execution result, if the execution is success. Otherwise; default value or throws the exception.</returns>
        public static TResult InvokeWithRetry<TResult>(this Func<TResult> execute, Predicate<Exception> transientExceptionFilter, params ushort[] retrySpans)
        {
            if (transientExceptionFilter is null)
            {
                return execute.InvokeWithRetry(retrySpans);
            }

            if (execute is null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            var intervals = GetIntervalsSafe(retrySpans);
            foreach (var interval in intervals)
            {
                try
                {
                    return execute.Invoke();
                }
                catch (Exception ex)
                {
                    if (!transientExceptionFilter(ex))
                    {
                        // If this isn't a transient error or we shouldn't retry,
                        // rethrow the exception.
                        throw;
                    }

                    Task.Delay(interval).Wait();
                }
            }

            return execute.Invoke();
        }

        /// <summary>
        /// Invokes an action with retry. If an exception is occured, retry several times.
        /// </summary>
        /// <typeparam name="TResult">The type of invoking the method result.</typeparam>
        /// <param name="execute">The invoking action.</param>
        /// <param name="retrySpans">The time spans to retry. The unit of them is milliseconds.</param>
        /// <returns>An execution result, if the execution is success. Otherwise; default value or throws the exception.</returns>
        public static TResult InvokeWithRetry<TResult>(this Func<TResult> execute, params ushort[] retrySpans)
        {
            if (execute is null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            var intervals = GetIntervalsSafe(retrySpans);
            foreach (var interval in intervals)
            {
                try
                {
                    return execute.Invoke();
                }
                catch (Exception)
                {
                    Task.Delay(interval).Wait();
                }
            }

            return execute.Invoke();
        }

        #region Asynchronously

        /// <summary>
        /// Runs a task with retry. If an exception is occured, retry several times.
        /// </summary>
        /// <param name="task">The running task.</param>
        /// <param name="transientExceptionFilter">
        /// The check logic whether the exception thrown is a transient exception.
        /// Retry invoking the action, if the check result is <c>true</c>. Otherwise; throws the exception.
        /// </param>
        /// <param name="retrySpans">The time spans to retry. The unit of them is milliseconds.</param>
        public static async Task RunAsyncWithRetry(Task task, Predicate<IEnumerable<Exception>> transientExceptionFilter, params ushort[] retrySpans)
        {
            if (transientExceptionFilter is null)
            {
                await RunAsyncWithRetry(task, retrySpans);
                return;
            }

            if (task is null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            var intervals = GetIntervalsSafe(retrySpans);
            foreach (var interval in intervals)
            {
                try
                {
                    await task;
                    return;
                }
                catch (AggregateException ae)
                {
                    if (!transientExceptionFilter(ae.InnerExceptions))
                    {
                        // If this isn't a transient error or we shouldn't retry,
                        // rethrow the exception.
                        throw;
                    }

                    await Task.Delay(interval);
                }
            }
            
            await task;
        }

        /// <summary>
        /// Runs a task with retry. If an exception is occured, retry several times.
        /// </summary>
        /// <param name="task">The running task.</param>
        /// <param name="retrySpans">The time spans to retry. The unit of them is milliseconds.</param>
        /// <exception cref="ArgumentNullException"><paramref name="task"/> is <c>null</c>.</exception>
        public static async Task RunAsyncWithRetry(Task task, params ushort[] retrySpans)
        {
            if (task is null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            var intervals = GetIntervalsSafe(retrySpans);
            foreach (var interval in intervals)
            {
                try
                {
                    await task;
                    return;
                }
                catch (AggregateException)
                {
                    await Task.Delay(interval);
                }
            }
            
            await task;
        }

        /// <summary>
        /// Invokes an action asynchronously with retry. If an exception is occured, retry several times.
        /// </summary>
        /// <param name="onAction">The invoking action.</param>
        /// <param name="transientExceptionFilter">
        /// The check logic whether the exception thrown is a transient exception.
        /// Retry invoking the action, if the check result is <c>true</c>. Otherwise; throws the exception.
        /// </param>
        /// <param name="retrySpans">The time spans to retry. The unit of them is milliseconds.</param>
        /// <exception cref="ArgumentNullException"><paramref name="onAction"/> is <c>null</c>.</exception>
        public static async Task InvokeAsyncWithRetry(Action onAction, Predicate<IEnumerable<Exception>> transientExceptionFilter, params ushort[] retrySpans)
        {
            if (onAction is null)
            {
                throw new ArgumentNullException(nameof(onAction));
            }

            await RunAsyncWithRetry(new Task(onAction), transientExceptionFilter, retrySpans);
        }

        /// <summary>
        /// Invokes an action asynchronously with retry. If an exception is occured, retry several times.
        /// </summary>
        /// <param name="onAction">The invoking action.</param>
        /// <param name="retrySpans">The time spans to retry. The unit of them is milliseconds.</param>
        /// <exception cref="ArgumentNullException"><paramref name="onAction"/> is <c>null</c>.</exception>
        public static async Task InvokeAsyncWithRetry(Action onAction, params ushort[] retrySpans)
        {
            if (onAction is null)
            {
                throw new ArgumentNullException(nameof(onAction));
            }

            await RunAsyncWithRetry(new Task(onAction), retrySpans);
        }

        /// <summary>
        /// Runs a task with retry. If an exception is occured, retry several times.
        /// </summary>
        /// <typeparam name="TResult">The type of invoking the method result.</typeparam>
        /// <param name="task">The running task.</param>
        /// <param name="transientExceptionFilter">
        /// The check logic whether the exception thrown is a transient exception.
        /// Retry invoking the action, if the check result is <c>true</c>. Otherwise; throws the exception.
        /// </param>
        /// <param name="retrySpans">The time spans to retry. The unit of them is milliseconds.</param>
        /// <returns>An execution result, if the execution is success. Otherwise; default value or throws the exception.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task"/> is <c>null</c>.</exception>
        public static async Task<TResult> RunAsyncWithRetry<TResult>(Task<TResult> task, Predicate<IEnumerable<Exception>> transientExceptionFilter, params ushort[] retrySpans)
        {
            if (transientExceptionFilter is null)
            {
                return await RunAsyncWithRetry(task, retrySpans);
            }

            if (task is null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            var intervals = GetIntervalsSafe(retrySpans);
            foreach (var interval in intervals)
            {
                try
                {
                    var result = await task;
                    return result;
                }
                catch (AggregateException ae)
                {
                    if (!transientExceptionFilter(ae.InnerExceptions))
                    {
                        // If this isn't a transient error or we shouldn't retry,
                        // rethrow the exception.
                        throw;
                    }

                    await Task.Delay(interval);
                }
            }
            
            return await task;
        }

        /// <summary>
        /// Runs a task with retry. If an exception is occured, retry several times.
        /// </summary>
        /// <typeparam name="TResult">The type of invoking the method result.</typeparam>
        /// <param name="task">The running task.</param>
        /// <param name="retrySpans">The time spans to retry. The unit of them is milliseconds.</param>
        /// <returns>An execution result, if the execution is success. Otherwise; default value or throws the exception.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task"/> is <c>null</c>.</exception>
        public static async Task<TResult> RunAsyncWithRetry<TResult>(Task<TResult> task, params ushort[] retrySpans)
        {
            if (task is null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            var intervals = GetIntervalsSafe(retrySpans);
            foreach (var interval in intervals)
            {
                try
                {
                    var result = await task;
                    return result;
                }
                catch (AggregateException)
                {
                    await Task.Delay(interval);
                }
            }

            return await task;
        }

        /// <summary>
        /// Invokes an action asynchronously with retry.
        /// If an exception that occured is a transient error or the invoker wants to retry, retry several times.
        /// </summary>
        /// <typeparam name="TResult">The type of invoking the method result.</typeparam>
        /// <param name="execute">The invoking action.</param>
        /// <param name="transientExceptionFilter">
        /// The check logic whether the exception thrown is a transient exception.
        /// Retry invoking the action, if the check result is <c>true</c>. Otherwise; throws the exception.
        /// </param>
        /// <param name="retrySpans">The time spans to retry. The unit of them is milliseconds.</param>
        /// <returns>An execution result, if the execution is success. Otherwise; default value or throws the exception.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="execute"/> is <c>null</c>.</exception>
        public static async Task<TResult> InvokeAsyncWithRetry<TResult>(Func<TResult> execute, Predicate<IEnumerable<Exception>> transientExceptionFilter, params ushort[] retrySpans)
        {
            if (execute is null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            return await RunAsyncWithRetry(new Task<TResult>(execute), transientExceptionFilter, retrySpans);
        }

        /// <summary>
        /// Invokes an action asynchronously with retry. If an exception is occured, retry several times.
        /// </summary>
        /// <typeparam name="TResult">The type of invoking the method result.</typeparam>
        /// <param name="execute">The invoking action.</param>
        /// <param name="retrySpans">The time spans to retry. The unit of them is milliseconds.</param>
        /// <returns>An execution result, if the execution is success. Otherwise; default value or throws the exception.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="execute"/> is <c>null</c>.</exception>
        public static async Task<TResult> InvokeAsyncWithRetry<TResult>(Func<TResult> execute, params ushort[] retrySpans)
        {
            if (execute is null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            return await RunAsyncWithRetry(new Task<TResult>(execute), retrySpans);
        }

        #endregion

        #endregion

        #region Private Methods

        private static ushort[] GetIntervalsSafe(ushort[] specifiedSpans)
        {
            if (specifiedSpans is null || specifiedSpans.Length == 0)
            {
                return DefaultSpans;
            }

            return specifiedSpans;
        }

        #endregion
    }
}
