module GameOfLife

type CellState = Dead | Alive
type CellNeighborInfo = CellNeighborInfo of CellState * int
type Grid = Grid of CellState[,]   
type GenerationId = GenerationId of int
type Generation =
    | Generation of Grid*GenerationId
    static member FromGrid grid =
        Generation (grid,GenerationId 1)


let mappings = 
    [ for x in [-1 .. 1] do
        for y in [-1 .. 1] do
            yield (x,y) ]

let project (a,b) =
    mappings |> List.map (fun (x,y) -> (a+x,b+y))



 
let private getNeighbors (Grid g) row col =
    let gridWidth = g |> Array2D.length2
    let gridHeight = g |> Array2D.length1
 
    let getPotentialNeighbors =
        [for r in [row-1 .. row+1] do
            for c in [col-1 .. col+1] do
                yield r,c]
 
    let isValidCoordinate (r,c) =
        r >= 0 && r < gridHeight &&
        c >= 0 && c < gridWidth &&
        (r,c) <> (row,col)
 
    getPotentialNeighbors |> List.filter isValidCoordinate
 
let private getCellInfo (g:Grid) row col =
    let (Grid grid) = g
    let getCellValue (r,c) =
        match Array2D.get grid r c with
        | Dead -> 0
        | Alive -> 1
    let aliveNeighbors = getNeighbors g row col |> List.sumBy getCellValue
    CellNeighborInfo (Array2D.get grid row col, aliveNeighbors)
 
let rec computeGenerations gen =
    let computeNewCell r c _ =
        let (Generation (g,_)) = gen
        match getCellInfo g r c with
        | CellNeighborInfo (Alive,x) when x=2 || x=3 -> Alive
        | CellNeighborInfo (Dead,3) -> Alive
        | _ -> Dead
 
    seq {
        yield gen

        let (Generation (Grid arr,GenerationId id)) = gen
        let newArray = Array2D.mapi computeNewCell arr
        let newGeneration = Generation (Grid newArray, GenerationId (id+1))
            
        yield! computeGenerations newGeneration
    }

