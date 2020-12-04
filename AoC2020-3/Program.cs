using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Read Map
string[] map = await File.ReadAllLinesAsync("input");
int lineLength = map[0].Length;
var cumlativeTrees = new List<int>();
var slopes = new List<(int x, int y)>{
    (1,1),
    (3,1),
    (5,1), // Use for part 1
    (7,1),
    (1,2)
};

bool CheckTree(string line, int x) => line?[x] == '#';

foreach(var slope in slopes)
{
    int trees = 0;
    for (int x = 0, y = 0; y + slope.y < map.Length;)
    {
        x = (x + slope.x) % lineLength;
        y = y + slope.y;
        if (CheckTree(map?[y], x))
            trees++;
    }
    cumlativeTrees.Add(trees);
}

Console.WriteLine("Trees: {0}", cumlativeTrees.Aggregate((a, i) => a * i));