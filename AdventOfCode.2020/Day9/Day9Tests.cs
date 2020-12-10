using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCode._2020.Day9
{
    [TestFixture]
    public class Day9Tests
    {
        [Test]
        public void Part1()
        {
            var answer = Day9.Part1();
            answer.Should().NotBe(147);
            answer.Should().Be(1398413738L);
        }
        
        [Test]
        public void Part2()
        {
            var answer = Day9.Part2();
            answer.Should().NotBe(2796827476L);
            answer.Should().NotBe(57);
            answer.Should().Be(169521051L);
        }
    }
}