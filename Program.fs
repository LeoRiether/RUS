open System
open System.Linq
open Parser
open DataStructure

let dataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
let dataFile = dataFolder + "/rus.data"

[<Literal>]
let HelpString = """R.U.S. -- Randomized Unsolved Stack

Usage:
    rus [command] [options]

Commands:
    help             Displays this help message
    push <url>       Pushes an URL onto the stack
    pop <url>        Pops an URL from the stack
    peek             Peeks a random item from near the top of the stack
    data-file        Prints the location of the RUS file

Examples:
    rus push https://codeforces.com/problemset/problem/4/A
    rus peek
    rus pop http://codeforces.com/problemset/problem/4/A
"""

let currentRus = lazy (RUS.readFromFile dataFile)

let doPeek () =
    let rus = currentRus.Force()
    let result =
        RUS.peek rus
        |> Option.defaultValue "The stack is empty"

    RUS.saveToFile dataFile rus
    result

let doPush (Url url) =
    let rus = currentRus.Force()
    RUS.push url rus
    RUS.saveToFile dataFile rus

let doPop (Url url) =
    let rus = currentRus.Force()
    let result = RUS.pop url rus
    RUS.saveToFile dataFile rus
    result

let execute = function
    | Help -> printfn "%s" HelpString
    | DataFile -> printfn "%s" dataFile

    | Peek ->
        let item = doPeek ()
        printfn "%s" item

    | Push (Url x) ->
        doPush (Url x)
        printfn "Pushed <%s> onto the stack!" x

    | Pop (Url x) ->
        let result = doPop (Url x)
        match result with
        | RUS.Popped -> printfn "Popped <%s> from the stack!" x
        | RUS.ItemNotFound -> printfn "Item <%s> wasn't on the stack! No changes made" x

[<EntryPoint>]
let main argv =
    let command = parseArgs (List.ofArray argv)

    match command with
    | Ok cmd -> execute cmd
    | Error e -> eprintfn "%s" e

    0
