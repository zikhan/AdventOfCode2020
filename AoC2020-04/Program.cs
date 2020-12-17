using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

int validPassports = 0;
var requiredFields = new Policy[]
{
    ("byr", (string x) => int.Parse(x) is >= 1920 and <= 2002),
    ("iyr", (string x) => int.Parse(x) is >= 2010 and <= 2020),
    ("eyr", (string x) => int.Parse(x) is >= 2020 and <= 2030),
    ("hgt", (string x) => x[^2..] switch
    {
        "cm" => int.Parse(x[..^2]) is >= 150 and <= 193,
        "in" => int.Parse(x[..^2]) is >= 59 and <=76 , 
        _ => false
    }),
    ("hcl", (string x) => Regex.IsMatch(x, @"#[0-9|a-f]{6}")),
    ("ecl", (string x) => x switch
    {
        "amb" => true,
        "blu" => true,
        "brn" => true,
        "gry" => true,
        "grn" => true,
        "hzl" => true,
        "oth" => true,
        _ => false
    }),
    ("pid", (string x) => Regex.IsMatch(x, @"^\d{9}$"))
};

var optionalFields = new Policy[] {
    ("cid", null)
};

var passportsRaw = await File.ReadAllTextAsync("input.txt");

var passports = passportsRaw.Split("\n\n").Select(x => x.Replace(" ", "\n"));

foreach (var passport in passports)
{
    var match = Regex.Match(passport, @"^(\w{3}):(.+)$", RegexOptions.Multiline);
    var compliance = new bool[requiredFields.Length];
    do
    {
        string fieldCode = match.Groups[1].Value;
        string fieldValue = match.Groups[2].Value;
        int check = GetIndex(requiredFields, fieldCode);

        if (check == -1)
        {
            if (GetIndex(optionalFields, fieldCode) == -1)
                break;
            else
                continue;
        }

        if (!compliance[check] && (requiredFields[check].policy == null || requiredFields[check].policy(fieldValue)))
            compliance[check] = true;
        else
            break;

    } while ((match = match.NextMatch()).Success);

    if (compliance.All(x => x))
        validPassports++;
}

Console.WriteLine("Valid Passports: {0}", validPassports);

static int GetIndex(Policy[] fields, string fieldCode)
{
    return fields.Select((x, i) => new { x.field, i }).FirstOrDefault(x => x.field == fieldCode)?.i ?? -1;
}

internal struct Policy
{
    public string field;
    public Func<string, bool> policy;

    public Policy(string field, Func<string, bool> policy)
    {
        this.field = field;
        this.policy = policy;
    }

    public static implicit operator (string field, Func<string, bool> policy)(Policy value)
    {
        return (value.field, value.policy);
    }

    public static implicit operator Policy((string field, Func<string, bool> policy) value)
    {
        return new Policy(value.field, value.policy);
    }
}