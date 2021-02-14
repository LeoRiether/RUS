module DataStructure

open System.IO

[<Struct>]
type RUS =
    { Data: ResizeArray<string> }

module RUS =
    let readFromFile file =
        { Data = File.ReadAllLines(file) |> ResizeArray }

    let saveToFile file rus =
        File.WriteAllLines(file, rus.Data)
