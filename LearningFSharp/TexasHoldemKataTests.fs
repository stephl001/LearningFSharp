namespace LearningFSharp

module TexasHoldemKataTests =

    open System
    open TexasHoldemKata
    open PokerHandParser
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

