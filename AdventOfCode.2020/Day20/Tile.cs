﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Common.Utils;
using FluentAssertions;
using Microsoft.Extensions.Primitives;

namespace AdventOfCode._2020.Day20
{
    internal class Tile
    {
        // hardcoded for ease, could generalise later...
        private readonly int Size;
        public long Id { get; set; }

        public Rotation Rotation { get; set; } = Rotation.None;

        public readonly List<List<bool>> Spaces;

        #region PreCalculatedForPerformance

        public readonly Oriented<long> EdgesAsLongs;


        // options arise from flipping each side
        public readonly Oriented<(long, long)> EdgeOptions;

        #endregion

        public Tile(IReadOnlyList<string> inputLines)
        {
            Size = inputLines.Last().Length;
            Id = long.Parse(inputLines[0].Split()[1].Replace(":", ""));

            Spaces = Init();
            for (var y = 0; y < Size; y++)
            {
                for (var x = 0; x < Size; x++)
                {
                    Spaces[y][x] = inputLines[y + 1][x] == '#';
                }
            }

            EdgesAsLongs = new Oriented<long>(ordinal => ordinal switch
            {
                Ordinal.North => EdgeAsLong(Row(0)),
                Ordinal.South => EdgeAsLong(Row(Size - 1)),
                Ordinal.West => EdgeAsLong(Column(0)),
                Ordinal.East => EdgeAsLong(Column(Size - 1)),
                _ => throw new ArgumentOutOfRangeException(nameof(ordinal))
            });

            EdgeOptions = new Oriented<(long, long)>(ordinal =>
            {
                var selfLongs = EdgesAsLongs;
                return ordinal switch
                {
                    Ordinal.North => (selfLongs.North, Reverse(selfLongs.North)),
                    Ordinal.South => (selfLongs.South, Reverse(selfLongs.South)),
                    Ordinal.West => (selfLongs.West, Reverse(selfLongs.West)),
                    Ordinal.East => (selfLongs.East, Reverse(selfLongs.East)),
                    _ => throw new ArgumentOutOfRangeException(nameof(ordinal))
                };
            });
        }

        private Tile(Tile original, Rotation rotation)
        {
            Id = original.Id;
            Spaces = rotation switch
            {
                Rotation.None => original.Spaces,
                Rotation.Right90 => Right90(original.Spaces),
                Rotation.Right180 => Right90(Right90(original.Spaces)),
                Rotation.Left90 => Right90(Right90(Right90(original.Spaces))),
                _ => throw new ArgumentOutOfRangeException(nameof(rotation), rotation, null)
            };
        }

        private Tile(long id, List<List<bool>> spaces)
        {
            Id = id;
            Spaces = spaces;
        }

        public Tile Rotate(Rotation rotation)
        {
            var newGrid = rotation switch
            {
                Rotation.None => Spaces,
                Rotation.Right90 => Right90(Spaces),
                Rotation.Right180 => Right90(Right90(Spaces)),
                Rotation.Left90 => Right90(Right90(Right90(Spaces))),
                _ => throw new ArgumentOutOfRangeException(nameof(rotation), rotation, null)
            };

            return new Tile(Id, newGrid);
        }

        public Tile Flip(Flip2D flip2D)
        {
            var newGrid = flip2D switch
            {
                Flip2D.None => Spaces,
                Flip2D.Horizontal => FlipHorizontal(Spaces),
                Flip2D.Vertical => FlipVertical(Spaces),
                _ => throw new ArgumentOutOfRangeException(nameof(flip2D))
            };
            return new Tile(Id, newGrid);
        }


        public List<List<bool>> Right90(List<List<bool>> original)
        {
            var rotated = Init();

            for (var y = 0; y < Size; y++)
            {
                for (var x = 0; x < Size; x++)
                {
                    rotated[y][x] = original[Size - x - 1][y];
                }
            }

            return rotated;
        }


        public List<List<bool>> FlipHorizontal(List<List<bool>> original)
        {
            var rotated = Init();

            for (var y = 0; y < Size; y++)
            {
                for (var x = 0; x < Size; x++)
                {
                    rotated[y][x] = original[y][Size - x - 1];
                }
            }

            return rotated;
        }

        public List<List<bool>> FlipVertical(List<List<bool>> original)
        {
            var rotated = Init();
            for (var y = 0; y < Size; y++)
            {
                for (var x = 0; x < Size; x++)
                {
                    rotated[y][x] = original[Size - y - 1][x];
                }
            }

            return rotated;
        }

        private List<List<bool>> Init()
        {
            return Enumerable.Range(0, Size)
                .Select(_ => Enumerable.Range(0, Size).Select(_ => false).ToList()).ToList();
        }

        public int Count => Spaces.Sum(row => row.Count(c => c));

        public int CountExceptBorders()
        {
            return Spaces.Skip(1).Take(8).Sum(row => row.Skip(1).Take(8).Count(x => x));
        }

        public long EdgeAsLong(List<bool> edge)
        {
            var sRep = edge.Aggregate("", (current, c) => current + (c ? '1' : '0'));
            return Convert.ToInt64(sRep, 2);
        }

        public long Reverse(long edge)
        {
            var sRep = Convert.ToString(edge, 2);
            sRep = sRep.PadLeft(10, '0');
            var reversed = sRep.Reverse().CharsToString();
            return Convert.ToInt64(reversed, 2);
        }

        public List<bool> Row(int i) => Spaces[i];
        public List<bool> Column(int i) => Spaces.Select(l => l[i]).ToList();

        public static bool AnyPairwiseMatch((long, long) one, (long, long) two) =>
            one.Item1 == two.Item1 || one.Item1 == two.Item2 || one.Item2 == two.Item1 || one.Item2 == two.Item2;

        public int MatchesInAnyOrientation(Tile other, Ordinal ordinal)
        {
            var sides = EdgeOptions.Get(ordinal);
            return other.EdgeOptions.Map((otherOptions, _) => AnyPairwiseMatch(sides, otherOptions) ? 1 : 0)
                .ToList()
                .Sum();
        }

        /// <summary>
        /// North, South, West, East
        /// </summary>
        /// <param name="others"></param>
        /// <returns></returns>
        public Oriented<int> PotentialNeighbours(List<Tile> others)
        {
            others = others.Where(o => o.Id != Id).ToList();

            int CountMatches(Ordinal side)
            {
                return others.Sum(o => MatchesInAnyOrientation(o, side));
            }

            return new Oriented<int>(CountMatches);
        }

        public Oriented<List<long>> IdsOfPotentialNeighbours(List<Tile> others)
        {
            others = others.Where(o => o.Id != Id).ToList();

            List<long> NeighbourIds(Ordinal side)
            {
                return others.Where(o => MatchesInAnyOrientation(o, side) != 0).Select(o => o.Id).ToList();
            }

            return new Oriented<List<long>>(NeighbourIds);
        }

        public string ToString()
        {
            var sb = new StringBuilder();
            foreach (var row in Spaces)
            {
                foreach (var item in row)
                {
                    sb.Append(item ? '#' : '.');
                }

                sb.Append(Environment.NewLine);
            }

            return sb.ToString().Trim();
        }
    }
}