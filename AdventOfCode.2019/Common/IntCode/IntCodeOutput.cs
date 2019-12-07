using System.Collections.Generic;

namespace AdventOfCode._2019.Common.IntCode
{
    public class IntCodeFinalOutput
    {
        public IEnumerable<int> FinalState { get; set; }
        public IEnumerable<int> Outputs { get; set; }
    }

    public class IntCodeOutput
    {
        public IEnumerable<int> CurrentState { get; set; }
        public int? Output { get; set; }
        public bool IsComplete { get; set; }
    }
}