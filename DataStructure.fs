module DataStructure

open System.IO
open System.Linq

let rng = System.Random()

[<Measure>] type Percent
let headsProbability = 60<Percent>

type Flip = Heads | Tails
let coinFlip () =
    if rng.Next(0, 100) < int headsProbability
    then Heads
    else Tails


[<Struct>]
type RUS =
    { Data: ResizeArray<string> }

module RUS =
    let readFromFile file =
        try
            { Data = File.ReadAllLines(file) |> ResizeArray }
        with
            | :? FileNotFoundException -> { Data = new ResizeArray<_>() }

    let saveToFile file rus =
        File.WriteAllLines(file, rus.Data)

    // Push an item to the RUS and randomized it a bit
    let push item rus =
        let mutable rus = rus
        rus.Data.Add(item)

        // Keep moving the last element backwards if we keep flipping heads
        let mutable i = rus.Data.Count - 1
        while i-1 >= 0 && coinFlip () = Heads do
            let (x, y) = rus.Data.[i-1], rus.Data.[i]
            rus.Data.[i] <- x
            rus.Data.[i-1] <- y
            i <- i - 1

    // Remove an item from the RUS
    type PopResult = Popped | ItemNotFound
    let pop item rus =
        let removed = rus.Data.Remove(item)
        if removed then Popped else ItemNotFound

    // Peek an item from approximately the top of the stack
    // Modifies the RUS to make subsequent peeks better
    let peek rus =
        if rus.Data.Count = 0 then None else

        // Pop the last item
        let last = rus.Data.Last()
        rus.Data.RemoveAt(rus.Data.Count - 1)

        // And push it back in
        push last rus

        // Then, get some random item next to the top
        let mutable i = rus.Data.Count - 1
        while i-1 >= 0 && coinFlip () = Heads do
            i <- i - 1

        Some rus.Data.[i]

