// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open System.IO
open System.Threading.Tasks

let distance (min:int) (max:int): int = (max - min + 1) / 2

let rec whichHalf (input: string) (min: int) (max: int) : int =
    let min, max =
        match input.[0] with
        | 'F' | 'L' -> (min, max - distance min max )
        | 'B' | 'R' -> (min + distance min max, max)
        | _ -> min, min

    if min = max
    then
        min
    else
        whichHalf input.[1..] min max;

let seatId (input: string) : int =
    let row = whichHalf input.[..6] 0 127
    let column = whichHalf input.[^2..] 0 7
    row * 8 + column

[<EntryPoint>]
let main argv =
    async {
        let! input =
            if Array.isEmpty argv
            then "input.txt" |> File.ReadAllLinesAsync |> Async.AwaitTask
            else Task.FromResult(argv) |> Async.AwaitTask

        // Part 1
        // Seq.map seatId input
        // |> Seq.max
        // |> printfn "Max SeatId: %i"

        let plus1andMinus1Exists (array: int []) seatId : bool=
            Array.contains (seatId + 1) array && Array.contains (seatId-1) array

        let boardingPasses = Array.Parallel.map seatId input
                             |> Array.sort

        [| for r in 0..127 do
             for c in 0..7 ->
               r * 8 + c |]
        |> Array.except boardingPasses
        |> Array.filter (plus1andMinus1Exists boardingPasses)
        |> Array.exactlyOne
        |> printfn "My Seat: %d"

    }
    |> Async.RunSynchronously

    0
