using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

var rawInput = await File.ReadAllTextAsync("input.txt");

var rangeRegExes = new Regex(@"^([\w| ]+): (\d+\-\d+) or (\d+\-\d+)$", RegexOptions.Multiline);

var validNumbers = new List<int>();
var fields = new Dictionary<string, List<int>>();

var matches = rangeRegExes.Matches(rawInput);

foreach(Match match in matches){
    var rangeSet= new List<int>();
    foreach (var group in match.Groups.Values.TakeLast(2))
    {
        var values = group.Value.Split('-');
        var min = int.Parse(values[0]);
        var max = int.Parse(values[1]);

        var range = Enumerable.Range(min, max-min+1);
        validNumbers.AddRange(range);
        validNumbers = validNumbers.Distinct().ToList();

        rangeSet.AddRange(range);
    }
    fields.Add(match.Groups[1].Value, new List<int>(rangeSet));
}

var nearbyIndex = rawInput.IndexOf("nearby tickets:\n");
var nearbyTickets = rawInput.Substring(nearbyIndex).Replace("nearby tickets:\n","").Split('\n', StringSplitOptions.TrimEntries);

int errorCount = 0;

var validNearbyTickets = new List<List<int>>();

foreach (var tix in nearbyTickets)
{
    var ticketNumbers = tix.Split(',').Select(int.Parse);
    var errors = ticketNumbers.Except(validNumbers);
    errorCount += errors.Sum();
    if (errors.Count() == 0)
        validNearbyTickets.Add(ticketNumbers.ToList());
}

Console.WriteLine("Part 1 Error Rate: {0}", errorCount);

var fieldMap = new Dictionary<string, List<int>>(); // fieldName mapped to possible fieldNumber

for (int fieldNumber = 0; fieldNumber < validNearbyTickets.First().Count; fieldNumber++)
{
    var testNumbers = validNearbyTickets.Select(x => x[fieldNumber]).Distinct();
    foreach (var field in fields/*.where(kv => !fieldmap.containskey(kv.key))*/)
    {
        if (testNumbers.Intersect(field.Value).Count() == testNumbers.Count())
        {
            if (fieldMap.ContainsKey(field.Key))
                fieldMap[field.Key].Add(fieldNumber);
            else
                fieldMap.Add(field.Key, new List<int>{fieldNumber});
        }
    }
}

void Uniquify(Dictionary<string, List<int>> possibleMaps, Dictionary<string, int> retDict)
{
    var mapsWith1Opt = possibleMaps.Where(kv => kv.Value.Count == 1);
    var removeIndexes = new List<int>();
    foreach (var map in mapsWith1Opt)
    {
        var index =  map.Value.Single();
        retDict.Add(map.Key, index);
        removeIndexes.Add(index);
    }
    var dict = possibleMaps.Where(kv => kv.Value.Count > 1)
                           .Select(kv => (kv.Key, kv.Value.Except(removeIndexes).ToList()))
                           .ToDictionary(x => x.Key, x => x.Item2);
    if (dict.Count == 0)
        return;
    Uniquify(dict, retDict);
}

var forRealMap = new Dictionary<string, int>();
Uniquify(fieldMap, forRealMap);

var yourTixIndex = rawInput.LastIndexOf("your ticket:\n");
var yourTicket = rawInput.Replace("your ticket:\n","").Substring(yourTixIndex).Split("\n\n")[0].Trim().Split(',', StringSplitOptions.TrimEntries).Select(ulong.Parse).ToArray();

var accumulator = 1ul;
foreach (var field in forRealMap.Where(kv => kv.Key.StartsWith("departure")))
{
    accumulator *= yourTicket[field.Value];
}

Console.WriteLine("Part 2: {0}", accumulator);