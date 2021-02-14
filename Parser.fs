module Parser

type Url = Url of string
type PushData = Url
type PopData = Url

type Command =
    | Help
    | DataFile
    | Peek
    | Push of PushData
    | Pop of PopData

let parseArgs = function
    | [] | "help"::_ | "--help"::_ -> Ok Help

    | "peek"::_ -> Ok Peek

    | ["push"] -> Error "push needs at least one argument"
    | "push"::url::_ -> Ok (Push (Url url))

    | ["pop"] -> Error "pop needs at least one argument"
    | "pop"::url::_ -> Ok (Pop (Url url))

    | "data-file"::_ -> Ok DataFile

    | unknown::_ -> Error (sprintf "Unknown command \"%s\"" unknown)