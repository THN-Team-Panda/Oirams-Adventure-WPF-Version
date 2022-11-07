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
        /// Stop the current loop execution
        /// </summary>
        private bool cancelToken = false;

        /// <summary>
        /// Determ if the loop is already running
        /// </summary>
        private bool isRunning = false;

        /// <summary>
        /// List of all methods to call in the loop.
        /// </summary>
        public event DispatchedItem ?Events;

        /// <summary>
        /// System.TimeSpan instance minimum time for each iteration.
        /// </summary>
        public TimeSpan MinRunTime { get; set; }

        /// <summary>
        /// Each event has to follow the LoopElement delegate pattern.
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
        /// Starts the execution loop in a async function.
        /// </summary>
        /// <exception cref="MissingFieldException">MinRunTime must be set.</exception>
        public void Start()
        {
            if (MinRunTime == TimeSpan.Zero)
                throw new MissingFieldException("MinRunTime is zero!");

            if (isRunning)
                return;

            cancelToken = false;

            ExecuteLoop();
        }

        /// <summary>
        /// Stops the execution by using the cancelToken.
        /// Note: It will finish the current running events.
        /// </summary>
        public void Stop() => cancelToken = true;

        /// <summary>
        /// Called as an async loop to execute the event list
        /// with a minimum execution time of MinRunTime for each iteration step.
        /// Can only be stopped with the LoopDispatcher.Stop() method.
        /// </summary>
        private async void ExecuteLoop()
        {
            isRunning = true;

            Stopwatch sw = new Stopwatch();

            while (!cancelToken)
            {
                sw.Restart();

                // Fix
                Events?.Invoke();

                // Check the token a second time
                if (cancelToken)
                    break;

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

            isRunning = false;
            Debug.WriteLine(isRunning);
        }
    }
}
