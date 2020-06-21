using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AgateLib.Diagnostics
{
    public class Scenario : IDisposable
    {
        private string scenarioName;
        private Logger log;
        private Stopwatch watch = new Stopwatch();
        private List<ScenarioStep> steps = new List<ScenarioStep>();
        private long lastTime = 0;

        public Scenario(string name)
        {
            this.scenarioName = name;
            this.log = LogManager.GetLogger(name);

            watch.Start();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            Stop();
        }

        /// <summary>
        /// Call when a step is complete.
        /// </summary>
        /// <param name="stepName"></param>
        public void Step(string stepName)
        {
            if (!watch.IsRunning)
            {
                throw new InvalidOperationException("Scenario has completed and should not be further marked.");
            }

            long time = watch.ElapsedMilliseconds;
            long delta = time - lastTime;

            var step = new ScenarioStep
            {
                Name = stepName,
                Delta = delta,
                Elapsed = time,
            };

            lock (steps)
            {
                steps.Add(step);
            }

            lastTime = time;
        }

        public void Stop()
        {
            Step("stop");

            watch.Stop();

            log.Info($"{scenarioName} complete.");

            for (int i = 0; i < steps.Count; i++)
            {
                log.Info($"     +{steps[i].Delta,6} = {steps[i].Elapsed,6} ms | {steps[i].Name}");
            }
        }
    }

    public class ScenarioStep
    {
        /// <summary>
        /// The name of this step.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The amount of time this step took after the last one.
        /// </summary>
        public long Delta { get; set; }

        /// <summary>
        /// The amount of time it took to complete this step, starting from the beginning of the scenario.
        /// </summary>
        public long Elapsed { get; set; }
    }
}
