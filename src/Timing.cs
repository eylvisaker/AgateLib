using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib
{
    /// <summary>
    /// Static class which handles timing.  This is often used
    /// to update object positions and animations.
    /// </summary>
    public static class Timing
    {
        private static StopWatch mAppTimer = new StopWatch();

        private delegate void TimerDelegate();

        private static event TimerDelegate PauseAllTimersEvent;
        private static event TimerDelegate ResumeAllTimersEvent;
        private static event TimerDelegate ForceResumeAllTimersEvent;

        /// <summary>
        /// Returns the number of seconds since the application started.
        /// </summary>
        public static double TotalSeconds
        {
            get { return mAppTimer.TotalSeconds; }
        }
        /// <summary>
        /// Returns the number of milliseconds since the application started.
        /// </summary>
        public static double TotalMilliseconds
        {
            get
            {
                return mAppTimer.TotalMilliseconds;
            }
        }

        /// <summary>
        /// Increments the pause counter.
        /// If the counter is greater than zero, the timer won't advance.
        /// </summary>
        public static void Pause()
        {
            mAppTimer.Pause();
        }
        /// <summary>
        /// Decrements the pause counter.
        /// If the pause counter is zero the timer will begin advancing.
        /// </summary>
        public static void Resume()
        {
            mAppTimer.Resume();
        }

        /// <summary>
        /// Sets the pause counter to zero, causing the timer to begin advancing
        /// regardless of how many calls to Pause() are made.
        /// </summary>
        public static void ForceResume()
        {
            mAppTimer.ForceResume();
        }

        /// <summary>
        /// Calls Pause() on all timers, and fires the PauseAllTimersEvent.
        /// </summary>
        public static void PauseAllTimers()
        {
            PauseAllTimersEvent();
        }

        /// <summary>
        /// Calls Resume() on all timers, and fires the ResumeAllTimersEvent.
        /// </summary>
        public static void ResumeAllTimers()
        {
            ResumeAllTimersEvent();
        }

        /// <summary>
        /// Calls ForceResume on all timers, and fires the ResumeAllTimersEvent.
        /// You probably don't want to use this one much.
        /// </summary>
        public static void ForceResumeAllTimers()
        {
            ForceResumeAllTimersEvent();
            ResumeAllTimersEvent();
        }

        /// <summary>
        /// Class which represents a StopWatch.
        /// A StopWatch can be paused and reset independently of other
        /// StopWatches.
        /// </summary>
        public class StopWatch : IDisposable
        {
            double mStartTime = Core.Platform.GetTime();
            int mPause = 0;
            double mPauseTime = 0;

            /// <summary>
            /// Constructs a timer object, and immediately begins 
            /// keeping track of time.
            /// </summary>
            public StopWatch()
                : this(true)
            {

            }
            /// <summary>
            /// Constructs a timer object.
            /// If the timer starts paused, a call to Resume() must be made
            /// for it to begin keeping track of time.
            /// </summary>
            /// <param name="autostart">Pass true to immediately begin keeping track of time.
            /// False to pause the timer initially.</param>
            public StopWatch(bool autostart)
            {
                PauseAllTimersEvent += Pause;
                ResumeAllTimersEvent += Resume;
                ForceResumeAllTimersEvent += ForceResume;

                if (autostart == false)
                {
                    Pause();
                }


            }

            /// <summary>
            /// Destroys this timer.
            /// </summary>
            public void Dispose()
            {
                PauseAllTimersEvent -= Pause;
                ResumeAllTimersEvent -= Resume;
                ForceResumeAllTimersEvent -= ForceResume;
            }

            /// <summary>
            /// Returns the number of seconds since the timer started.
            /// </summary>
            public double TotalSeconds
            {
                get
                {
                    return TotalMilliseconds / 1000.0;
                }
            }

            /// <summary>
            /// Returns the number of ticks (milliseconds) since the timer started.
            /// </summary>
            public double TotalMilliseconds
            {
                get
                {
                    if (mPause == 0)
                        return Core.Platform.GetTime() - mStartTime;
                    else
                        return mPauseTime - mStartTime;
                }
            }

            /// <summary>
            /// Resets the timer to zero.
            /// </summary>
            public void Reset()
            {
                mStartTime = Core.Platform.GetTime();

                if (mPause > 0)
                    mPauseTime = mStartTime;
            }

            /// <summary>
            /// Increments the pause counter.
            /// If the counter is greater than zero, the timer won't advance.
            /// </summary>
            public void Pause()
            {
                mPause += 1;

                if (mPause == 1)
                    mPauseTime = Core.Platform.GetTime();


            }
            /// <summary>
            /// Decrements the pause counter.
            /// If the pause counter is zero the timer will begin advancing.
            /// </summary>
            public void Resume()
            {
                mPause -= 1;

                if (mPause == 0)
                {
                    mStartTime += Core.Platform.GetTime() - mPauseTime;

                    mPauseTime = 0;
                }
                else if (mPause < 0)
                    mPause = 0;
            }


            /// <summary>
            /// Sets the pause counter to zero, causing the timer to begin advancing
            /// regardless of how many calls to Pause() are made.
            /// </summary>
            public void ForceResume()
            {
                if (mPause <= 0)
                    return;

                mPause = 1;

                Resume();
            }
        }
    }
}