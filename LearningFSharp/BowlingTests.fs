module BowlingTests

open BowlingKata
open FsUnit
open NUnit.Framework

[<TestCase(20,0,0)>]
[<TestCase(12,10,300)>]
[<TestCase(21,5,150)>]
[<TestCase(20,4,80)>]
let ``Make sure consecutive identical throws will result in a properly scored game``(throws,knockedPins,score) =
    let card = (ScoreCard (List.replicate throws knockedPins))
    scoreGame card |> should equal score


