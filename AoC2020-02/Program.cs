using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
;
int success = 0;
var filePath = "passwords.txt";
await foreach (var item in ParsePolicyPasswords(filePath))
{
    if (CheckPolicy(item)){
        success++;
    }
}

Console.WriteLine($"Good passwords: {success}");

static async IAsyncEnumerable<PolicyPassword> ParsePolicyPasswords(string filePath)
{
    using var file = File.OpenRead(filePath);
    using var reader = new StreamReader(file);
    string line;
    while ((line = await reader.ReadLineAsync()) is not null)
    {
        yield return ParseLine(line);
    }
}

static PolicyPassword ParseLine(string line)
{
    const string regexPattern = @"(\d+)-(\d+)\ ([a-z]):\ (.+)";
    var match = Regex.Match(line, regexPattern);
    return new PolicyPassword
    {
        min = int.Parse(match.Groups[1].Value),
        max = int.Parse(match.Groups[2].Value),
        policy = match.Groups[3].Value[0],
        password = match.Groups[4].Value
    };
}

static bool CheckPolicy(PolicyPassword policy)
{
    // Policy Changed for Part 2
    return policy.password[policy.min - 1] == policy.policy ^ policy.password[policy.max - 1] == policy.policy;
}

public record PolicyPassword
{
    public int min { get; init; }
    public int max { get; init; }
    public char policy { get; init; }
    public string password { get; init; }

    public override string ToString() => $"{min}-{max} {policy}: {password}";
}
