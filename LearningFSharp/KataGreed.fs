namespace LearningFSharp

module KataGreed =
 
    type D6 = Two | Three | Four | Five | Six | One
    type Roll =
        | Roll of D6 list
        static member create dl =
            if List.length dl <= 6
            then Some (Roll dl)
            else None    

    type Match =
        | Nothing
        | Single of D6
        | Triple of D6
        | FourOfAKind of D6
        | FiveOfAKind of D6
        | SixOfAKind of D6
        | ThreePairs
        | Straight
 
    let getGroupedDice roll =
        let toDiceGroup (d,diceCount) = (Seq.length diceCount,d)
        roll
        |> Seq.groupBy id
        |> Seq.map toDiceGroup
        |> Seq.sortByDescending id
        |> Seq.toList
 
    let getMatch (Roll roll) =
        let rec matchGroupedDice = function
        | [(1,_);(1,_);(1,_);(1,_);(1,_);(1,_)] -> Straight
        | [(2,_);(2,_);(2,_)] -> ThreePairs
        | [(6,d)] -> SixOfAKind d
        | (5,d)::_ -> FiveOfAKind d
        | (4,d)::_ -> FourOfAKind d
        | (3,d)::_ -> Triple d
        | (_,d)::rest when (d<>One && d<>Five) -> matchGroupedDice rest
        | (_,d)::_ when d=One || d=Five -> Single d
        | _ -> Nothing
 
        getGroupedDice roll |> matchGroupedDice
 
    (* Points Calculation *)
    let private (|DiceValue|) = function
    | One -> 10 | Two -> 2 | Three -> 3 | Four -> 4
    | Five -> 5 | Six -> 6
 
    let private pointsMultiplicator multFactor (DiceValue dv) = dv * multFactor
    let private matchOneMultiplicator   = pointsMultiplicator 10
    let private matchThreeMultiplicator = pointsMultiplicator 100
    let private matchFourMultiplicator  = (matchThreeMultiplicator >> (*) 2)
    let private matchFiveMultiplicator  = (matchThreeMultiplicator >> (*) 4)
    let private matchSixMultiplicator   = (matchThreeMultiplicator >> (*) 8)
 
    let pointsFromMatch = function
    | Straight -> 1200
    | ThreePairs -> 800
    | SixOfAKind d -> matchSixMultiplicator d
    | FiveOfAKind d -> matchFiveMultiplicator d
    | FourOfAKind d -> matchFourMultiplicator d
    | Triple d -> matchThreeMultiplicator d
    | Single d -> matchOneMultiplicator d
    | Nothing -> 0

    let getScore = 
        getMatch >> pointsFromMatch
(*
    let charToDie = function
    | '1' -> One | '2' -> Two | '3' -> Three | '4' -> Four | '5' -> Five | '6' -> Six

    module List = 
        let rec insertions x = function
            | []             -> [[x]]
            | (y :: ys) as l -> (x::l)::(List.map (fun x -> y::x) (insertions x ys))

        let rec permutations = function
            | []      -> seq [ [] ]
            | x :: xs -> Seq.concat (Seq.map (insertions x) (permutations xs))

    let rec cartesian lstlst =
        match lstlst with
        | h::[] ->
            List.fold (fun acc elem -> [elem]::acc) [] h
        | h::t ->
            List.fold (fun cacc celem ->
                (List.fold (fun acc elem -> (elem::celem)::acc) [] h) @ cacc
                ) [] (cartesian t)
        | _ -> []

    let intToDie = function
    | 1 -> One | 2 -> Two | 3 -> Three | 4 -> Four | 5 -> Five | 6 -> Six

    let getAllPossibleRolls =
        List.replicate 6 [1..6]
        |> (cartesian >> List.map List.sort)
        |> List.distinct 
        |> List.map (List.map intToDie) 
        |> Seq.choose Roll.create 

        *)