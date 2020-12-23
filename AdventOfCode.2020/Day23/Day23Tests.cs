using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCode._2020.Day23
{
    [TestFixture]
    public class Day23Tests
    {
        [Test]
        public void Part1_Example()
        {
            var answer = Day23.Part1(true);
            answer.Should().Be(67384529);
        }

        [Test]
        public void Part1_SmallExample()
        {
            var cups = "389125467".Select(x => int.Parse(x.ToString())).ToList();

            var answer = Day23.CupsToLong(Day23.PlayCrabCubs(cups, 0, 10));

            answer.Should().Be(583741926);
        }
        
        [Test]
        public void Part2_Example()
        {
            var answer = Day23.Part2(true);
            answer.Should().Be(-1);
        }
        
        [Test]
        public void Part1()
        {
            var answer = Day23.Part1();
            answer.Should().NotBe(527861349L);
            answer.Should().Be(0);
        }
        
        [Test]
        public void Part2()
        {
            var answer = Day23.Part2();
            answer.Should().NotBe(-2);
            answer.Should().Be(0);
        }
    }
}