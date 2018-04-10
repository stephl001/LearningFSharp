namespace LearningFSharp

module TexasHoldemKata =

    type Suit = Spade | Club | Diamond | Heart
    type Rank = 
        | Two | Three | Four | Five | Six | Seven | Eight
        | Nine | Ten | Jack | Queen | King | Ace
    type Card = Card of Rank*Suit
    type Hand = Hand of Card list
    
    type SingleKicker = SingleKicker of Rank
    type DoubleKicker = DoubleKicker of Rank*Rank
    type TripleKicker = TripleKicker of Rank*Rank*Rank
    type QuadKicker = QuadKicker of Rank*Rank*Rank*Rank
    
    type PokerHand =
        | HighCard of Rank*QuadKicker
        | SinglePair of Rank*TripleKicker
        | DoublePair of Rank*Rank*SingleKicker
        | ThreeOfAKind of Rank*DoubleKicker
        | Straight of Rank
        | Flush of Rank
        | FullHouse of Rank*Rank
        | FourOfAKind of Rank*SingleKicker
        | StraightFlush of Rank

    let rec comb = function
    | 0, _ -> [[]]
    | _, [] -> []
    | k, (x::xs) -> List.map ((@) [x]) (comb (k-1,xs)) @ comb (k,xs)

    let handArrangements (Hand cards) = 
        comb (5,cards) 
        |> List.distinctBy (List.map (fun (Card (r,_)) -> r))
        |> List.map Hand