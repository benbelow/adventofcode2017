using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCode._2020.Day24
{
    [TestFixture]
    public class Day24Tests
    {
        [Test]
        public void Part1_Example()
        {
            var answer = Day24.Part1(true);
            answer.Should().Be(10);
        }
        
        [Test]
        public void Part2_Example()
        {
            var answer = Day24.Part2(true);
            answer.Should().Be(2208);
        }
        
        [Test]
        public void Part1()
        {
            var answer = Day24.Part1();
            answer.Should().NotBe(-1);
            answer.Should().Be(354);
        }
        
        [Test]
        public void Part2()
        {
            var answer = Day24.Part2();
            answer.Should().NotBe(-2);
            answer.Should().Be(3608);
        }
    }
}