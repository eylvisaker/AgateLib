using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace AgateLib.Algorithms.PathFinding
{
	[ContractClassFor(typeof(IAStarMap<>))]
	abstract class AStarMapContract<T> : IAStarMap<T>
	{
		public void ReportProgress(AStarState<T> task)
		{
			Contract.Requires<ArgumentNullException>(task != null);
			throw new NotImplementedException();
		}

		public int CalculateHeuristic(T location, List<T> destination)
		{
			Contract.Requires(destination != null);

			throw new NotImplementedException();
		}

		public IEnumerable<T> GetAvailableSteps(AStarState<T> task, T location)
		{
			Contract.Requires<ArgumentNullException>(task != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			throw new NotImplementedException();
		}

		public int GetStepCost(T target, T start)
		{
			throw new NotImplementedException();
		}
	}
}
