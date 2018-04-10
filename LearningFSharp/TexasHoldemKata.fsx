#load "TexasHoldemKata.fs"
open LearningFSharp.TexasHoldemKata

let parseRank = function
| '2' -> Two | '3' -> Three | '4' -> Four | '5' -> Five | '6' -> Six
| '7' -> Seven | '8' -> Eight | '9' -> Nine | 'T' -> Ten | 'J' -> Jack
| 'Q' -> Queen | 'K' -> King | 'A' -> Ace | _ -> failwith "Wrong Rank Character"
    
let parseSuit = function
| 's' -> Spade | 'c' -> Club | 'd' -> Diamond | 'h' -> Heart
| _ -> failwith "Wrong Suit Character"

let parseCardEntry = function
| [r;s] -> (parseRank r, parseSuit s) |> Card
| _ -> failwith "Invalid card entry"

let parseHand (handStr:string) = 
    handStr.Split ' ' 
    |> Array.map (List.ofSeq >> parseCardEntry)
    |> List.ofArray
    |> Hand
