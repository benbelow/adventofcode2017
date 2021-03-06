using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCode._2019.Day13
{
    [TestFixture]
    public class Day13Tests
    {
        [Test]
        public void Part1()
        {
            Day13.Part1().Should().Be(298);
        }
        
        [Test]
        public void Part2()
        {
            Day13.Part2().Should().Be(13956);
        }
    }
}