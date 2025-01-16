// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;

namespace GaltonBoard
{
    public class GaltonBoard(int width = 800, int height = 400, int numBalls = 10000)
    {
        private const string ImagePath = "galton_board.ppm";
        private readonly int _width = width > 0 ? width : throw new ArgumentOutOfRangeException(nameof(width));
        private readonly int _height = height > 0 ? height : throw new ArgumentOutOfRangeException(nameof(height));
        private readonly int _numBalls = numBalls > 0 ? numBalls : throw new ArgumentOutOfRangeException(nameof(numBalls));
        private readonly List<int> _distribution = Enumerable.Repeat(0, width).ToList();
        private readonly Random _random = new();

        public void Simulate() => Enumerable.Range(0, _numBalls).ToList().ForEach(_ => _distribution[CalculateBin()]++);

        private int CalculateBin() => Math.Clamp(_width / 2 + Enumerable.Range(0, _height).Sum(_ => _random.Next(2) == 0 ? -1 : 1), 0, _width - 1);

        public void GeneratePpm()
        {
            var maxFrequency = _distribution.Max();
            var ppmContent = $"P2\n{_width} {_height}\n255\n";
            for (int y = _height - 1; y >= 0; y--)
            {
                var line = "";
                for (int x = 0; x < _width; x++)
                {
                    var barHeight = (int)((double)_distribution[x] / maxFrequency * _height);
                    line += y < barHeight ? "255 " : "0 ";
                }
                ppmContent += line.Trim() + "\n";
            }
            File.WriteAllText(ImagePath, ppmContent);
        }

        public class Program
        {
            public static void Main()
            {
                var board = new GaltonBoard();
                board.Simulate();
                board.GeneratePpm();
            }
        }
    }
}
