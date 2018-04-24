#load "TexasHoldemKata.fs"
#load "PokerHandParser.fs"
#load "PokerScorer.fs"
#load "PokerHandPrinter.fs"
open LearningFSharp
open TexasHoldemKata
open PokerHandParser
open PokerScorer 
open PokerHandPrinter

let fastParse = parseCards >> matchCardList
//let cardListToString cardList =
//    cardList |> List.map
 
//let getHandStringRep = function
//| Folded { Hole=(Hole (h1,h2)); CommonCards=(CommunityCards cardList) } ->
//    h1::h2::cardList |> cardListToString
//| Winning hand ->
//| NonWinning hand ->