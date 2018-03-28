namespace LearningFSharp

module BowlingTestsLunch =

    open BowlingKata
    open FsUnit
    open NUnit.Framework
    open FsCheck

    let scoreFromList = ScoreCard >> scoreGame

    [<Test>]
    let firstTest() =
        10 |> should equal 10

