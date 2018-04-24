namespace LearningFSharp

module TexasHoldemKataTests =

    open System
    open TexasHoldemKata
    open PokerHandParser
    open PokerScorer
    open FsUnit
    open NUnit.Framework
    open FsCheck
    open FsCheck.NUnit
    
    let allCards =
        [for r in "23456789TJQKA" do
            for s in "scdh" do
                yield ([|r;s|] |> String) ]

    [<Test>]
    let ``Make sure we can parse all 52 distinct cards of a deck``()  =
        allCards 
        |> List.map parseCard 
        |> List.distinct 
        |> should haveLength 13

    [<Test>]
    let ``Make sure we can parse a single poker hand``()  =
        parseCards (String.Join(" ", allCards))
        |> List.distinct 
        |> should haveLength 13

    [<Test>]
    let ``There should be 9 different poker hands``()  =
        let possibleHands = [
            "3s 8h As 9d 2c"    //High card
            "4s 6c 4c 9h Td"    //One pair
            "Ah Ts Tc 6c Ac"    //Two pairs
            "8s 8h 9c 2h 8d"    //Three of a kind
            "5h 6h 8c 7d 9d"    //Straight
            "Td 7d 8d Jd Ad"    //Flush
            "4s 5h 2c 5d 5s"    //Full House
            "Jc Jh 6s Jd Js"    //Four of a kind
            "9h Th Jh Kh Qh"    //Straight flush
        ]
        possibleHands 
        |> List.map scoreHand 
        |> List.distinct 
        |> should haveLength 9

    //[<Test>]
    //let ``Matched poker hands should yield card in then proper order``()  =
    //    let possibleHands = [
    //        "3s 8h As 9d 2c","As 9d 8h 3s 2c"    //High card
    //        "4s 6c 4c 9h Td",""    //One pair
    //        "Ah Ts Tc 6c Ac",""    //Two pairs
    //        "8s 8h 9c 2h 8d",""    //Three of a kind
    //        "5h 6h 8c 7d 9d",""    //Straight
    //        "Td 7d 8d Jd Ad",""    //Flush
    //        "4s 5h 2c 5d 5s",""    //Full House
    //        "Jc Jh 6s Jd Js",""    //Four of a kind
    //        "9h Th Jh Kh Qh",""    //Straight flush
    //    ]
    //    possibleHands 
    //    |> List.map (fst>>scoreHand)
    //    |> List.distinct 
    //    |> should haveLength 9

