using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Diagnostics
{
    /// <summary>
    /// Provides some basic diagnostic measurements.
    /// </summary>
    public static class Profiler
    {
        /// <summary>
        /// Measures the amount of time an action takes and writes it to the log.
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="action"></param>
        public static async Task MeasureAsync(string actionName, Func<Task> action)
        {
            Stopwatch watch = new Stopwatch();

            Log.Info($"Beginning task {actionName}...");
            watch.Start();

            try
            {
                await action();

                Log.Info($"Completed {actionName} in {watch.Elapsed.TotalSeconds} s.");
            }
            catch (Exception e)
            {
                Log.Info($"Task {actionName} faulted after {watch.Elapsed.TotalSeconds} s.");
                throw;
            }
        }
        /// <summary>
        /// Measures the amount of time an action takes and writes it to the log.
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="action"></param>
        public static async Task<T> MeasureAsync<T>(string actionName, Func<Task<T>> action)
        {
            Stopwatch watch = new Stopwatch();

            Log.Info($"Beginning task {actionName}...");
            watch.Start();

            try
            {
                T result = await action();

                Log.Info($"Completed {actionName} in {watch.Elapsed.TotalSeconds} s.");

                return result;
            }
            catch (Exception e)
            {
                Log.Info($"Task {actionName} faulted after {watch.Elapsed.TotalSeconds} s.");
                throw;
            }
        }

        /// <summary>
        /// Measures the amount of time an action takes and writes it to the log.
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="action"></param>
        public static T Measure<T>(string actionName, Func<T> action)
        {
            Stopwatch watch = new Stopwatch();

            Log.Info($"Beginning task {actionName}...");
            watch.Start();

            try
            {
                T result = action();

                Log.Info($"Completed {actionName} in {watch.Elapsed.TotalSeconds} s.");

                return result;
            }
            catch (Exception e)
            {
                Log.Info($"Task {actionName} faulted after {watch.Elapsed.TotalSeconds} s.");
                throw;
            }
        }

        /// <summary>
        /// Measures the amount of time an action takes and writes it to the log.
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="action"></param>
        public static void Measure(string actionName, Action action)
        {
            Stopwatch watch = new Stopwatch();

            Log.Info($"Beginning task {actionName}...");
            watch.Start();

            try
            {
                action();

                Log.Info($"Completed {actionName} in {watch.Elapsed.TotalSeconds} s.");
            }
            catch (Exception e)
            {
                Log.Info($"Task {actionName} faulted after {watch.Elapsed.TotalSeconds} s.");
                throw;
            }
        }
    }
}
