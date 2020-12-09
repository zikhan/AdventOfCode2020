// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System.IO

// Find if number is a sum of 2 numbers in last x
let rec findIfValid (combinationElements: int64[]) (findNumber: int64): bool =
    if Array.isEmpty combinationElements
    then false
    else
        Array.allPairs combinationElements combinationElements
        |> Array.filter (fun (x,y) -> x <> y)
        |> Array.exists (fun (x,y) -> x+y = findNumber)

let rec part1 (preamble: int) (combinationElements: int64[]) : int64 =
    if findIfValid combinationElements.[..preamble] combinationElements.[preamble + 1]
    then
        part1 preamble combinationElements.[1..]
    else
        combinationElements.[preamble+1]

let rec findContiguousSet (elementsToSearch: int64[]) (testNumber: int64)=
    // Start at staring index and add each subsequent number
    // Repeat if sum is smaller than testNumber
    let mutable i = 0;
    while ((Array.sum elementsToSearch.[..i]) < testNumber ) do
        i <- i + 1

    // If sum is equal to testNumber, return the range of numbers added
    if (Array.sum elementsToSearch.[..i]) = testNumber
    then
        elementsToSearch.[..i]
    // If sum is greater than testNumber, then run this call again with the head ignored. It's more brute forcing!
    else
        findContiguousSet elementsToSearch.[1..] testNumber

let rec part2 (preamble: int) (originalElements:int64[]) (combinationElements: int64[]) : int64 =
    if findIfValid combinationElements.[..preamble] combinationElements.[preamble + 1]
    then
        part2 preamble originalElements combinationElements.[1..]
    else
        // Continguous set that equal preamble+1
        let continguousSet = findContiguousSet originalElements combinationElements.[preamble + 1]

        (Array.max continguousSet, Array.min continguousSet)
        ||> (+)


[<EntryPoint>]
let main argv =
    async{
        let! rawInput = File.ReadAllLinesAsync "input.txt" |> Async.AwaitTask

        let input =
            rawInput
            |> Array.map int64

        part1 25 input
        |> printfn ("Part 1: %i")

        part2 25 input input
        |> printfn ("Part 2: %i")
    } |> Async.RunSynchronously
    0 // return an integer exit code