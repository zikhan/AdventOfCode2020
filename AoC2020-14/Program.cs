using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

var input = await File.ReadAllLinesAsync("input.txt");

char[] bitMask = "".ToCharArray();
var mem = new Dictionary<string, ulong>();

var bitMaskRegex = new Regex(@"^mask = ([1|0|X]{36})$");
var registerRegex = new Regex(@"^mem\[(\d+)\] = (\d+)$");

foreach (var line in input)
{
    var memMatch = registerRegex.Match(line);
    if (memMatch.Success)
    {
        var register = memMatch.Groups[1].Value;
        var rawValue = memMatch.Groups[2].Value;

        var binArr = new BitArray(BitConverter.GetBytes(ulong.Parse(rawValue)));

        for (int i = 0; i < bitMask.Length; i++)
        {
            switch (bitMask[i])
            {
                case '0':
                    binArr[i] = false;
                    break;
                case '1':
                    binArr[i] = true;
                    break;
                case 'X':
                default:
                    break;
            }
        }

        var lastArray = new byte[8];
        binArr.CopyTo(lastArray, 0);
        mem[register] = BitConverter.ToUInt64(lastArray);
        continue;
    }

    var bitMatch = bitMaskRegex.Match(line);
    if (bitMatch.Success)
    {
        bitMask = bitMatch.Groups[1].Value.Reverse().ToArray();
    }
}

Console.WriteLine("Part 1 Memory Sum: {0}", mem.Values.Aggregate((acc, current) => acc + current));

var memReg = new Dictionary<ulong,ulong>();

foreach (var line in input)
{
    var memMatch = registerRegex.Match(line);
    if (memMatch.Success)
    {
        var value = ulong.Parse(memMatch.Groups[2].Value);

        var registerArray = new BitArray(BitConverter.GetBytes(ulong.Parse(memMatch.Groups[1].Value))).Cast<bool>().Select(x => x ? '1' : '0').ToArray();

        for (int i = 0; i < bitMask.Length; i++)
        {
            switch (bitMask[i])
            {
                case '1':
                case 'X':
                    registerArray[i] = bitMask[i];
                    break;
                case '0':
                default:
                    break;
            }
        }

        List<ulong> permutations = new List<ulong>();
        for (int i = 0; i < registerArray.Length; i++)
        {
            switch (registerArray[i])
            {
                case '0':
                    if (permutations.Count() == 0)
                        permutations.Add(0ul);
                    break;
                case '1':
                    if (permutations.Any())
                        permutations = permutations.Select(v => v + Convert.ToUInt64(Math.Pow(2, i))).ToList();
                    else
                        permutations.Add(Convert.ToUInt64(Math.Pow(2,i)));
                    break;
                case 'X':
                    if (permutations.Any())
                    {
                        // Add Nothing
                        var old = permutations.ToList();
                        // Add 2^i
                        permutations = permutations.Select(v => v + Convert.ToUInt64(Math.Pow(2,i))).ToList();
                        permutations.AddRange(old);
                    }
                    else {
                        permutations.Add(0ul);
                        permutations.Add(Convert.ToUInt64(Math.Pow(2,i)));
                    }
                    break;
            }
        }

        foreach (var reg in permutations)
        {
            memReg[reg] = value;
        }
        continue;
    }

    var bitMatch = bitMaskRegex.Match(line);
    if (bitMatch.Success)
        bitMask = bitMatch.Groups[1].Value.Reverse().ToArray();
}

Console.WriteLine("Part 2 Memory Sum: {0}", memReg.Values.Aggregate((acc, current) => acc + current));