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
        /// List of all methods to call in the loop.
        /// </summary>
        private List<LoopElement> functions = new List<LoopElement>();

        /// <summary>
        /// System.Threading.Tasks.Task instance to run the loop in another thread.
        /// </summary>
        private Task? task;

        /// <summary>
        /// Instance of System.Threading.CancellationTokenSource to cancel the running loop.
        /// </summary>
        private CancellationTokenSource cancelToken = new CancellationTokenSource();

        /// <summary>
        /// Priority label effects the method execution order.
        /// </summary>
        public enum Priority { Important, Secondary }

        /// <summary>
        /// System.TimeSpan instance minimum time for each iteration.
        /// </summary>
        public TimeSpan MinRunTime { get; set; }

        /// <summary>
        /// Each element has to follow the LoopElement delegate pattern.
        /// </summary>
        public delegate void LoopElement();

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
        /// Add a method with the GameEngine.LoopDispatcher.Priority Important
        /// to the execution queue.
        /// </summary>
        /// <param name="function">Delegate to add.</param>
        public void AddElement(LoopElement function)
        {
            AddElement(function, Priority.Important);
        }

        /// <summary>
        /// Add a method to the execution queue.
        /// </summary>
        /// <param name="function">Delegate to add.</param>
        /// <param name="priority">Important inserts the element at the beginning of the
        /// execution list. Secondary appends it.
        /// </param>
        public void AddElement(LoopElement function, Priority priority)
        {
            if (priority == Priority.Important)
                functions.Insert(0, function);
            else if (priority == Priority.Secondary)
                functions.Add(function);
        }

        /// <summary>
        /// Removes the given function from the execution queue.
        /// </summary>
        /// <param name="function">Method to remove.</param>
        public void RemoveElement(LoopElement function)
        {
            functions.Remove(function);
        }

        /// <summary>
        /// Starts the execution loop in a parallel task.
        /// </summary>
        /// <exception cref="MissingFieldException">MinRunTime must be set.</exception>
        public void Start()
        {
            if (MinRunTime == TimeSpan.Zero)
                throw new MissingFieldException("MinRunTime is Zero!");

            // Instance new task with the cancel token
            // https://stackoverflow.com/questions/33713298/c-sharp-run-a-loop-in-the-background
            task = new Task(ExecuteLoop, cancelToken.Token);

            task.Start();
        }

        /// <summary>
        /// Stops the execution by using the System.Threading.CancellationTokenSource.
        /// Note: It will finish the current running list element first.
        /// </summary>
        public void Stop()
        {
            cancelToken.Cancel();
        }

        /// <summary>
        /// Called from the System.Threading.Task to execute the LoopElement list
        /// with a minimum execution time of MinRunTime for each iteration step.
        /// Can only be stopped with the LoopDispatcher.Stop() method.
        /// </summary>
        private void ExecuteLoop()
        {
            Stopwatch sw = new Stopwatch();

            while (true)
            {
                sw.Restart();

                foreach (LoopElement function in functions)
                {
                    // Check the token status befor calling the function
                    if (cancelToken.IsCancellationRequested)
                        return;

                    function();
                }

                // Check the token a second time
                if (cancelToken.IsCancellationRequested)
                    return;

                // Calculate the total time of execution
                TimeSpan timeDiff = MinRunTime - sw.Elapsed;

                // Log if the execution takes more time than it is given for
                // each iteration step
                if (timeDiff < TimeSpan.Zero)
                    Debug.WriteLine("Execute game loop takes longer than the given TimeSpan!");
                else
                    // Fill up to get MinRunTime in total
                    Thread.Sleep(timeDiff);
            }
        }
    }
}
