open System.Text.RegularExpressions

type CellState = Dead | Alive
type Generation = Generation of CellState [,]
type GenerationData = { Id: int; Lines: int; Columns: int; Data: string list }
type InputReader = unit -> GenerationData

let (|GenerationId|_|) line = 
    let regex = new Regex(@"^Generation:\s(\d+)$")
    let m = regex.Match(line)
    if m.Success 
    then Some (m.Groups.Item(1).Value|>int)
    else None
let (|Dimension|_|) line =
    let regex = new Regex(@"^(\d+)\s(\d+)$")
    let m = regex.Match(line)
    if m.Success 
    then Some (m.Groups.Item(1).Value|>int,m.Groups.Item(2).Value|>int)
    else None

let readGenerationData = function
| (GenerationId id)::(Dimension d)::xs -> {Id=id; Lines=fst d; Columns=snd d; Data=xs}
| _ -> failwith "Could not read generation data"

let lineMapper (line:string) =
    let toCellState = function
    | '.' -> Dead | '*' -> Alive | _ -> failwith "Invalid character"

    line |> Seq.map toCellState |> List.ofSeq

let readGeneration (reader:InputReader) = 
    let generationData = reader()
    generationData.Data |> (List.map lineMapper >> array2D >> Generation)