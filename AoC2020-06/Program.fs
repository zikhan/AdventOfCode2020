// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open System.IO

// Input: List of Answers, Groups delinated by \n\n, individual in group by \n
// Part 1:
// Procedure: produce list of unique letters per group
//            Add lengths of all unique letters per group
// Output: Sum of the counts
// Part 2:
// Procedure: produce list of answers per group
//            intersect all individual response

let part1 (input: string[]) : int =
    let flattenGroup (x: string) =
        x.Replace("\n","")

    (Seq.sumBy (flattenGroup >> Seq.distinct >> Seq.length) input)

let part2 (groups: string[]) =
    // produce a set per individual in the group -> Set<char>[]
    let getSetsOfResponsesinGroup (group:string) =
        Array.map Set.ofSeq (group.Split('\n'))

    // for every group, intersect all sets in group and count elements in output
    Seq.sumBy (getSetsOfResponsesinGroup >> Set.intersectMany >> Set.count) groups

[<EntryPoint>]
let main argv =
    async {
        let! rawInput = File.ReadAllTextAsync "input.txt" |> Async.AwaitTask

        part1 (rawInput.Trim().Split("\n\n"))
        |> printfn "Part 1 Final Count: %i"

        part2 (rawInput.Trim().Split("\n\n"))
        |> printfn "Part 2 Final Count: %i"
    } |> Async.RunSynchronously
    0 // return an integer exit code