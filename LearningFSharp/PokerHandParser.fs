namespace LearningFSharp

module PokerHandParser =
    open TexasHoldemKata

    let private (|Rank|_|) = function
    | '2' -> Some Two | '3' -> Some Three | '4' -> Some Four | '5' -> Some Five 
    | '6' -> Some Six | '7' -> Some Seven | '8' -> Some Eight | '9' -> Some Nine 
    | 'T' -> Some Ten | 'J' -> Some Jack | 'Q' -> Some Queen | 'K' -> Some King 
    | 'A' -> Some Ace | _ -> None
   
    let private (|Suit|_|) = function
    | 's' -> Some Spade | 'c' -> Some Club | 'd' -> Some Diamond | 'h' -> Some Heart
    | _ -> None
 
    let private (|CardEntry|_|) = function
    | [Rank r;Suit s] -> Some (Card (r,s))
    | _ -> None

    let cardFromCardEntry = function
    | CardEntry c -> c
    | _ -> failwith "Invalid card entry"

    let getCardEntries (handStr:string) = 
        handStr.Split ' '
        |> Array.map List.ofSeq
        |> List.ofArray
        |> List.map cardFromCardEntry

    let private buildHandFromCards = function
    | h1::h2::xs ->
        match xs with
        | [] -> Hand (Hole (h1,h2),CommunityCards.Empty)
        | [f1;f2;f3] -> Hand (Hole (h1,h2),Flop (f1,f2,f3))
        | [t1;t2;t3;t4] -> Hand (Hole (h1,h2),Turn (t1,t2,t3,t4))
        | [r1;r2;r3;r4;r5] -> Hand (Hole (h1,h2),River (r1,r2,r3,r4,r5))
        | _ -> failwith "Invalid community card sequence"
    | _ -> failwith ""
 
    let parseCard (cardEntry:string) = 
        cardEntry |> (List.ofSeq >> cardFromCardEntry)
    
    let parseCards = getCardEntries
    let parseHand = getCardEntries >> buildHandFromCards
    let parseHands hands = 
        hands |> (Seq.map parseHand >> List.ofSeq)

