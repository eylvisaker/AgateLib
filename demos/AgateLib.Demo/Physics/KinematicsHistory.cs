using AgateLib.Physics.TwoDimensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Demo.Physics
{
    public class KinematicsHistory
    {
        private List<List<PhysicalParticle>> history = new List<List<PhysicalParticle>>();
        private int historyIndex = -1;

        public int Index
        {
            get => historyIndex;
            set
            {
                historyIndex = value;

                if (historyIndex < 0)
                    historyIndex = 0;

                if (historyIndex >= history.Count)
                    historyIndex = history.Count - 1;
            }
        }

        public IReadOnlyList<PhysicalParticle> State
            => history[historyIndex];

        public void AdvanceAndStoreHistory(IReadOnlyList<PhysicalParticle> particles)
        {
            historyIndex++;
            StoreHistory(particles);
        }

        public void StoreHistory(IReadOnlyList<PhysicalParticle> particles)
        {
            var historyItem = particles.Select(x => x.Clone()).ToList();

            if (historyIndex == history.Count)
            {
                history.Add(historyItem);
            }
            else
            {
                history[historyIndex] = historyItem;
            }
        }

        public void Clear()
        {
            history.Clear();
            historyIndex = -1;
        }
    }
}
