// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open System.IO

// Input Pattern: adj adj bags contain [ ## adj adj bag|bags (,...)].

// model: bagType = "adj adj"
//        bagCanContainType = seq {int * bagType}

// index model: [Contained: bagType] -> [ContainerBag(, ...)]

let bagRuleParse(rawRule: string) =
    let firstIndexSpace = rawRule.IndexOf(' ')
    let secondIndexSpace = rawRule.IndexOf(' ', firstIndexSpace+1);
    let containerBag = rawRule.[..(secondIndexSpace - 1)]
    let thirdIndexSpace = rawRule.IndexOf(' ', secondIndexSpace + 1)
    let fourthIndexSpace = rawRule.IndexOf(' ', thirdIndexSpace + 1);
    let containedRules = rawRule.[(fourthIndexSpace + 1)..].TrimEnd('.').Split(',')

    let bagRules = match containedRules.[0] with
                   | "no other bags" -> Array.empty
                   | _ -> [|for rule in containedRules do
                                let words = rule.Trim().Split(' ')
                                let count = words.[0] |> Int32.Parse
                                let bag = String.Join(' ',words.[1..2])
                                (count, bag)
                          |]

    (containerBag, bagRules)

let rec findContainerBags (allRules: (string * (int * string) array) array) (search: string) =
    let foundContainers = Array.filter (fun (_, bags) -> bags |> Array.exists (fun (_, bag) -> bag = search)) allRules
                          |> Array.map (fun (container, _bags) -> container) // This finds the first direct layer

    if (Array.isEmpty foundContainers)
    then foundContainers // No where to go
    else 
        Array.collect (findContainerBags allRules) foundContainers
        |> Array.append foundContainers

// Part 1 How many bag colors can eventually contain at least one shiny gold bag? 
let part1 (allRules: (string * (int * string) array) array) =
        let searchString = "shiny gold"

        findContainerBags allRules searchString
        |> Array.distinct
        |> Array.length
        |> printfn "Possible bags containing %s: %i" searchString

// Part 2 How many individual bags are required inside your single shiny gold bag?
let part2 (allRules: (string * (int * string) array) array) =
    printfn "TBD"

[<EntryPoint>]
let main argv =
    async {
        let! input = File.ReadAllLinesAsync "input.txt" |> Async.AwaitTask

        
        let allRules =  Array.map bagRuleParse input

        part1 allRules
        part2 allRules
    } |> Async.RunSynchronously
    0 // return an integer exit code