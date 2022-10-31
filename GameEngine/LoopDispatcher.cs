using System.Diagnostics;

namespace GameEngine
{
    /// <summary>
    /// Register methods to the class and run them in a
    /// minimum System.TimeSpan interval.
    /// </summary>
    public class LoopDispatcher
    {
        /// <summary>
        /// Instance of System.Threading.CancellationTokenSource to cancel the running loop.
        /// </summary>
        private bool cancelToken = false;

        /// <summary>
        /// List of all methods to call in the loop.
        /// </summary>
        public event DispatchedItem ?Events;

        /// <summary>
        /// System.TimeSpan instance minimum time for each iteration.
        /// </summary>
        public TimeSpan MinRunTime { get; set; }

        /// <summary>
        /// Each element has to follow the LoopElement delegate pattern.
        /// </summary>
        public delegate void DispatchedItem();

        /// <summary>
        /// Creates a GameEngine.LoopDispatcher instance.
        /// </summary>
        public LoopDispatcher() { }

        /// <summary>
        /// Creates a GameEngine.LoopDispatcher instance.
        /// </summary>
        public LoopDispatcher(TimeSpan minRunTime)
        {
            MinRunTime = minRunTime;
        }

        /// <summary>
        /// Starts the execution loop in a parallel task.
        /// </summary>
        /// <exception cref="MissingFieldException">MinRunTime must be set.</exception>
        public void Start()
        {
            if (MinRunTime == TimeSpan.Zero)
                throw new MissingFieldException("MinRunTime is zero!");

            if (Events == null)
                throw new MissingFieldException("No Events registered!");

            cancelToken = false;

            ExecuteLoop();
        }

        /// <summary>
        /// Stops the execution by using the System.Threading.CancellationTokenSource.
        /// Note: It will finish the current running list element first.
        /// </summary>
        public void Stop()
        {
            cancelToken = true;
        }

        /// <summary>
        /// Called from the System.Threading.Task to execute the LoopElement list
        /// with a minimum execution time of MinRunTime for each iteration step.
        /// Can only be stopped with the LoopDispatcher.Stop() method.
        /// </summary>
        private async void ExecuteLoop()
        {
            Stopwatch sw = new Stopwatch();

            while (!cancelToken)
            {
                sw.Restart();

                Events();

                // Check the token a second time
                if (cancelToken)
                    return;

                // Calculate the total time of execution
                TimeSpan timeDiff = MinRunTime - sw.Elapsed;

                // Log if the execution takes more time than it is given for
                // each iteration step
                if (timeDiff < TimeSpan.Zero)
                    Debug.WriteLine("Execute game loop takes longer than the given TimeSpan!");
                else
                    // Fill up to get MinRunTime in total
                    await Task.Delay(timeDiff);
            }
        }
    }
}
