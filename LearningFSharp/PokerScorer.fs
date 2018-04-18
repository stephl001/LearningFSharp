namespace LearningFSharp

module PokerScorer =
    open TexasHoldemKata
    open PokerHandParser

    let getCardList (Hand (Hole (c1,c2),commonCards)) =
        match commonCards with
        | Empty -> [c1;c2]
        | Flop (f1,f2,f3) -> [c1;c2;f1;f2;f3]
        | Turn (f1,f2,f3,f4) -> [c1;c2;f1;f2;f3;f4]
        | River (f1,f2,f3,f4,f5) -> [c1;c2;f1;f2;f3;f4;f5]
 
    let rec comb = function
    | 0, _ -> [[]]
    | _, [] -> []
    | k, (x::xs) -> List.map ((@) [x]) (comb (k-1,xs)) @ comb (k,xs)
 
    let handArrangements cards =
        comb (PokerHandCardsCount,cards)
        |> List.distinctBy (List.map (fun (Card (r,_)) -> r))

    let cardRank (Card (r,_)) = r
    let cardSuit (Card (_,s)) = s

    type CardList = Card list
    type GroupCardCount = int
    type GroupKey = GroupCardCount*Rank
    type GroupedCards = (GroupKey*CardList) list
    let groupCards (cards:Card list) : GroupedCards =
        cards 
        |> List.groupBy cardRank
        |> List.map (fun (r,l) -> (l.Length,r),l)
        |> List.sortDescending

    let sameSuit cards =
        (cards |> List.distinctBy cardSuit |> List.length) = 1

    let buildStraightOrFlushHand ((c1,c2,c3,c4,c5) as suite) =
        let isFlush = [c1;c2;c3;c4;c5] |> sameSuit
        if isFlush then StraightFlush suite else Straight suite
        
    let matchGroupedCards = function
    | [((_,Two),[c1]);(_,[c2]);(_,[c3]);((_,Five),[c4]);((_,Ace),[c5])] ->
        buildStraightOrFlushHand (c5,c1,c2,c3,c4)
    | [((_,r1),[c1]);(_,[c2]);(_,[c3]);(_,[c4]);((_,r2),[c5])] when r2-r1=4 -> 
        buildStraightOrFlushHand (c1,c2,c3,c4,c5)
    | [((4,_),[c1;c2;c3;c4]);(_,[kicker])] -> FourOfaKind ((c1,c2,c3,c4),kicker)
    | [((3,_),[c1;c2;c3]);(_,[c4;c5])] -> FullHouse ((c1,c2,c3),(c4,c5))
    | [((3,_),[c1;c2;c3]);(_,[k1]);(_,[k2])] -> ThreeOfaKind ((c1,c2,c3),(k1,k2))
    | _ -> Invalid

    let parseCardList = parseHand >> getCardList
    let matchCardList = groupCards >> matchGroupedCards
    let scoreHand = parseCardList >> matchCardList

