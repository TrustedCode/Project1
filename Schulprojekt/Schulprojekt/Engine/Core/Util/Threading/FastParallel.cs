#define PARALLEL_SAFETY

using System;
using System.Threading;


namespace Animation_Engine.Engine.Core.Util.Threading
{
    public class FastParallel : IDisposable
    {
        private Thread[] _worker;               // The worker Threads
        private Barrier _threadBarrier;         // Blockiert alles solange nicht 
        private ParallelState _currentState;    // Current parallel state reference

        private int _threadcount = 0;           // Current thread count.
        private bool _working = true;           // Threads are up?

        private object objLock = new object();  // Thread lock
        private int _count = 0;                 // Thread Id incremented by each thread in locked state (unique!)

        /// <summary>
        /// Creates an instance for easy use
        /// </summary>
        /// <param name="threads"></param>
        /// <returns></returns>
        public static FastParallel Instance(int threads)
        {
            FastParallel parallel = new FastParallel();
            parallel._threadcount = threads;
            parallel.Init();
            return parallel;
        }

        private void Init()
        {
            _worker = new Thread[_threadcount];
            _threadBarrier = new Barrier(_threadcount + 1);
            for (int i = 0; i < _worker.Length; i++)
            {
                _worker[i] = new Thread(new ThreadStart(Run));
                _worker[i].Start();
            }
        }

        /// <summary>
        /// Creates a new parallel state for later use.
        /// Splits the tasks into pieces and assigns them to each worker.
        /// </summary>
        /// <param name="from">Offset</param>
        /// <param name="to">Length</param>
        /// <param name="action">Function which will be called</param>
        /// <returns></returns>
        public ParallelState CreateState(int from, int to, Action<Int32> action)
        {
            int[] threadAssignments = new int[_threadcount * 2];
            ParallelState state = new ParallelState(action, threadAssignments);
            int absSize = to - from;
            int size = absSize / _threadcount;
            int lastSize = absSize - (size * (_threadcount - 1));
            int tick = 0;
            for (int i = 0; i < state.ThreadAssignments.Length; i += 2)
            {
                state.ThreadAssignments[i] = tick * size;
                if (i + 2 == state.ThreadAssignments.Length)
                {
                    state.ThreadAssignments[i + 1] = (size * tick) + lastSize;
                }
                else
                {
                    state.ThreadAssignments[i + 1] = (tick + 1) * size;
                }
                tick++;
            }
            return state;
        }

        /// <summary>
        /// Invokes a Loop event and forces all threads to run.
        /// </summary>
        /// <param name="from">Offset</param>
        /// <param name="to">Length</param>
        /// <param name="action">Function which will be called</param>
        public void Loop(int from, int to, Action<Int32> action)
        {
            Loop(CreateState(from, to, action));
        }

        /// <summary>
        /// Invokes a Loop event and forces all threads to run.
        /// </summary>
        /// <param name="state"></param>
        public void Loop(ParallelState state)
        {
            if (state.Action == null) return;

            _currentState = state;

            _threadBarrier.SignalAndWait(); // Start everything
            _threadBarrier.SignalAndWait(); // Wait for everything
        }

        /// <summary>
        /// Thread running function
        /// </summary>
        private void Run()
        {
            int threadId = 0;
            lock (objLock)
            {
                threadId = _count;
                _count += 2; // Increment by 2 because of the [from, to] layout
            }
            while (_working)
            {
                _threadBarrier.SignalAndWait(); // Wait for loop event
                if (_currentState == null)
                {
                    continue; // Usually it should break out of the thread after this
                }
#if PARALLEL_SAFETY
                try
                {
#endif
                    int start = _currentState.ThreadAssignments[threadId];
                    int end = _currentState.ThreadAssignments[threadId + 1];
                    for (int i = start; i < end; i++)
                    {
                        _currentState.Action(i);
                    }

#if PARALLEL_SAFETY
                }
                catch (IndexOutOfRangeException e)
                {
                    Console.WriteLine("Wrong ParallelState found, index ran out of range! {0}", e.ToString());
                }
#endif
                _threadBarrier.SignalAndWait(); // Signal finished
            }
            _threadBarrier.SignalAndWait(); // Signal thread exited
        }

        /// <summary>
        /// Stops the threads properly
        /// </summary>
        public void Stop()
        {
            if (_working)
            {
                _working = false;               // turn off threads
                _currentState = null;
                _threadBarrier.SignalAndWait(); // Signal threads continue
                _threadBarrier.SignalAndWait(); // Wait till threads have exited
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }

    /// <summary>
    /// Parallel state to store the threading values
    /// </summary>
    public class ParallelState
    {
        public int[] ThreadAssignments; // Stores [from, to] for each loop thread
        public Action<Int32> Action; // The called Function

        public ParallelState(Action<Int32> action, int[] threadAssignments)
        {
            Action = action;
            ThreadAssignments = threadAssignments;
        }
    }
}
