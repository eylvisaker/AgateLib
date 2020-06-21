using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace AgateLib.Randomizer
{
    public class RandomExtensionsUnitTests
    {
        private const int TestCount = 15000;

        private IRandom rnd;

        public RandomExtensionsUnitTests()
        {
            rnd = new Xoroshiro128pp(45);
        }

        [Fact]
        public void NextIntegerMaxBehavior()
        {
            for (int i = 0; i < TestCount; i++)
            {
                int value = rnd.NextInteger(10);

                value.Should().BeInRange(0, 9, $"Iteration {i} should be between 0-9");
            }
        }

        [Fact]
        public void WeightedDistributionSelections()
        {
            Dictionary<int, int> valueSelectionCounts = new Dictionary<int, int>();

            for (int i = 1; i < 10; i++)
            {
                valueSelectionCounts[i] = 0;
            }

            List<int> values = valueSelectionCounts.Keys.ToList();

            for (int i = 0; i < TestCount; i++)
            {
                valueSelectionCounts[rnd.PickOneWeighted(values, x => x)]++;
            }

            int weightSum = valueSelectionCounts.Values.Sum();
            int baseValue = valueSelectionCounts[1];

            StringBuilder distributionResult = new StringBuilder();
            bool anyOutOfRange = false;

            distributionResult.AppendLine();

            for (int i = 1; i < 10; i++)
            {
                int minValue = (int)((i - 0.5) * baseValue);
                int maxValue = (int)((i + 0.5) * baseValue);

                double pickFraction = valueSelectionCounts[i] / (double)weightSum;
                double minFraction = minValue / (double)weightSum;
                double maxFraction = maxValue / (double)weightSum;

                bool inRange = pickFraction >= minFraction
                            && pickFraction <= maxFraction;

                distributionResult.Append(
                    $"Item {i} picked {100 * pickFraction:0.00}% of the time  ");

                if (!inRange)
                {
                    distributionResult.Append(
                        $" - out of expected range {100 * minFraction}%, {100 * maxFraction}%");

                    anyOutOfRange = true;
                }

                distributionResult.AppendLine();
            }

            anyOutOfRange.Should().BeFalse(distributionResult.ToString());
        }
    }
}