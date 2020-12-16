using System;
using System.Collections.Generic;
using System.Linq;

var startingList = new[] {6,19,0,5,7,13,1};

var allSeen = new Dictionary<int, List<int>>();
var lastNumber = 0;

for (int i = 0; i < startingList.Length; i++)
{
        lastNumber = startingList[i];
        allSeen.Add(lastNumber, new List<int>{i});
}

for (int i = startingList.Length; i < 2020; i++)
{
    var lastSeenAge = 0;
    if (allSeen[lastNumber].Count > 1)
        lastSeenAge = Math.Abs(allSeen[lastNumber].TakeLast(2).Aggregate((acc,cur) => acc - cur));
    else
        lastSeenAge = 0;
    
    lastNumber = lastSeenAge;
    if (allSeen.ContainsKey(lastSeenAge))
        allSeen[lastSeenAge] = allSeen[lastSeenAge].Append(i).TakeLast(2).ToList();
    else
        allSeen.Add(lastSeenAge, new List<int>{i});
}

Console.WriteLine("Part 1: {0}", lastNumber);

for (int i = 2020; i < 30000000; i++)
{
    var lastSeenAge = 0;
    if (allSeen[lastNumber].Count > 1)
        lastSeenAge = Math.Abs(allSeen[lastNumber].Aggregate((acc,cur) => acc - cur));
    else
        lastSeenAge = 0;
    
    lastNumber = lastSeenAge;
    if (allSeen.ContainsKey(lastSeenAge))
        allSeen[lastSeenAge] = allSeen[lastSeenAge].TakeLast(1).Append(i).ToList();
    else
        allSeen.Add(lastSeenAge, new List<int>{i});
}

Console.WriteLine("Part 2: {0}", lastNumber);