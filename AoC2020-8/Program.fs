// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open System.IO

let parseProgram (rawInput: string array) =
    let parseLine (s: string) =
        (s.[..2], s.[3..] |> int)
    Array.map parseLine rawInput

let execute (instruction: string * int) (accumulator: int) : int * int =
    match instruction with
    | (op, count) when op = "acc" -> (accumulator + count, 1)
    | (op, count) when op = "jmp" -> (accumulator, count)
    | (op, _) when op = "nop" -> (accumulator, 1)
    | _ -> (accumulator, 1)

// Part 1 What's the accumulator prior to infinite loop

let rec runProgramPt1 (instructions: (string * int) array) (executedInstructions: int array) (currentInstruction: int) (accumulator: int): int =
    if Array.contains currentInstruction executedInstructions
    then
        accumulator
    else
        match execute instructions.[currentInstruction] accumulator with
        | (acc, next) ->
            runProgramPt1 instructions (Array.append executedInstructions [| currentInstruction |]) (currentInstruction + next) acc


let part1 (instructions: (string * int) array) =
    runProgramPt1 instructions Array.empty<int> 0 0
    |> printfn "Accumulator prior to loop %i"

// Part 2 Change 1 nop to jmp or 1 jmp to nop to fix the infinite loop. What's the accumulator 
let amount _ y =  y
let findInstructionToFlip (original: (string * int) array) (startingIndex: int): (string * int) array * int =
    let instructions = Array.copy original
    let nextTryIndex =
        if startingIndex = (Array.length instructions)
        then (Array.findIndexBack (fun (x, _) -> x = "jmp" || x = "nop") instructions)
        else (Array.findIndex (fun (x, _) -> x = "jmp" || x = "nop") instructions.[startingIndex..]) + startingIndex

    let swap s c =
        Array.set instructions nextTryIndex (s, c)

    match instructions.[nextTryIndex] with
    | (op, count) when op = "jmp" -> swap "nop" count
    | (op, count) when op = "nop" -> swap "jmp" count
    | a -> a |> ignore

    (instructions, nextTryIndex)

let rec runProgramPt2 (original: ((string * int) array * int )) (instructions: (string * int) array) (executedInstructions: int array) (currentInstruction: int) (accumulator: int): int =
    match execute instructions.[currentInstruction] accumulator with
    | (acc, next) ->
        // Test next instruction, if it's in it, rerun instructions with a nop in it's place
        if currentInstruction < (Array.length instructions) - 1
        then
            let nextInstruction = currentInstruction + next;
            if Array.contains nextInstruction executedInstructions
            then
                // Try the next one, cause I'm BRUTE FORCING IT! Cause I still suck at math
                let newOriginal = original ||> findInstructionToFlip
                runProgramPt2 (original ||> (fun x _ -> (x, (newOriginal ||> amount) + 1))) (newOriginal ||> (fun arr _ -> arr)) Array.empty<int> 0 0
            else
                runProgramPt2 original instructions (Array.append executedInstructions [| currentInstruction |]) nextInstruction acc
        else
            acc

let part2 (instructions: (string * int) array) =
    runProgramPt2 (instructions, 0) instructions Array.empty<int> 0 0
    |> printfn "Total Accumulation: %i"

[<EntryPoint>]
let main _ =
    async {
        let! rawInput = File.ReadAllLinesAsync "input.txt" |> Async.AwaitTask

        let program = parseProgram rawInput

        part1 program
        part2 program
    } |> Async.RunSynchronously
    0 // return an integer exit code
