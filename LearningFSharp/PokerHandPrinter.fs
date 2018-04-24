namespace LearningFSharp

module PokerHandPrinter =
    open TexasHoldemKata
    open PokerScorer

    let rankString = function
    | Two -> "2" | Three -> "3" | Four -> "4" | Five -> "5" | Six -> "6" | Seven -> "7"
    | Eight -> "8" | Nine -> "9" | Ten -> "T" | Jack -> "J" | Queen -> "Q" | King -> "K"
    | Ace -> "A"
    let suitString = function
    | Heart -> "h" | Spade -> "s" | Club -> "c" | Diamond -> "d"

    let printCard (Card (r,s)) =
        rankString r + suitString s

    let printCards = List.map printCard >> String.concat " "
    let pringHandOutcome handType cards = 
        sprintf "%s %s" (printCards cards) handType

    let printHand = function
    | HighCard (c1,c2,c3,c4,c5) -> [c1;c2;c3;c4;c5] |> pringHandOutcome "High Card"
    | OnePair ((c1,c2),(k3,k4,k5)) -> [c1;c2;k3;k4;k5] |> pringHandOutcome "One Pair"
    | DoublePair ((c1,c2),(c3,c4),k1) -> [c1;c2;c3;c4;k1] |> pringHandOutcome "Two Pairs"
    | ThreeOfaKind ((c1,c2,c3),(k1,k2)) ->  [c1;c2;c3;k1;k2] |> pringHandOutcome "Three of a kind"
    | Straight ((_,c1),c2,c3,c4,c5) -> [c1;c2;c3;c4;c5] |> pringHandOutcome "Straight"
    | Flush (c1,c2,c3,c4,c5) -> [c1;c2;c3;c4;c5] |> pringHandOutcome "Flush"
    | FullHouse ((c1,c2,c3),(c4,c5)) -> [c1;c2;c3;c4;c5] |> pringHandOutcome "Full House"
    | FourOfaKind ((c1,c2,c3,c4),k1) -> [c1;c2;c3;c4;k1] |> pringHandOutcome "Four of a kind"
    | StraightFlush ((_,c1),c2,c3,c4,c5) -> [c1;c2;c3;c4;c5] |> pringHandOutcome "Straight Flush"
    | Invalid -> "Invalid poker hand"

