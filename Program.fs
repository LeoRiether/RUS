open System
open System.Linq
open Parser
open DataStructure

let dataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
let dataFile = dataFolder + "/rus.data"

[<Literal>]
let HelpString = """R.U.S. – Randomized Unsolved Stack

Usage:
    rus [command] [options]

Commands:
    help             Displays this help message
    push <url>       Pushes an URL onto the stack
    pop <url>        Pops an URL from the stack
    peek             Peeks a random item from near the top of the stack

Examples:
    rus push https://codeforces.com/problemset/problem/4/A
    rus peek
    rus pop http://codeforces.com/problemset/problem/4/A
"""

let currentRus = lazy (RUS.readFromFile dataFile)

let doPeek () =
    let rus = currentRus.Force().Data
    let item = if rus.Count = 0
               then "the stack is empty"
               else rus.Last()
    printfn "%s" item

let doPush (Url url) =
    let rus = currentRus.Force()
    rus.Data.Add(url)
    RUS.saveToFile dataFile rus

let doPop data =
    let rus = currentRus.Force()
    if rus.Data.Count > 0 then
        rus.Data.RemoveAt(rus.Data.Count - 1)
    RUS.saveToFile dataFile rus

let execute = function
    | Help -> printfn "%s" HelpString
    | Peek -> doPeek ()
    | Push x -> doPush x
    | Pop x  -> doPop x

[<EntryPoint>]
let main argv =
    let command = parseArgs (List.ofArray argv)

    match command with
    | Ok cmd -> execute cmd
    | Error e -> failwith e

    0
