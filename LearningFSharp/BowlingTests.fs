namespace LearningFSharp

module BowlingTests =

    open BowlingKata
    open FsUnit
    open NUnit.Framework
    open FsCheck

    let scoreFromList = ScoreCard >> scoreGame

    [<TestCase(12,10,300)>]
    [<TestCase(21,5,150)>]
    let ``Make sure consecutive identical throws will result in a properly scored game``(throws,knockedPins,score) =
        List.replicate throws knockedPins |> scoreFromList |> should equal score

    [<Test>]
    let ``Make sure a game with alternating strikes and spares will yield 200 points``() =
        [10;5;5;10;5;5;10;5;5;10;5;5;10;5;5;10] |> scoreFromList |> should equal 200

    let openCardGenerator = 
        gen {
            let! throwList = 
                Gen.choose (0, 9) 
                |> Gen.two 
                |> Gen.filter (fun (a,b) -> (a+b)<10) 
                |> Gen.listOfLength 10 
                |> Gen.map (List.collect (fun (a,b) -> [a;b]))
            return (ScoreCard throwList)
        } |> Arb.fromGen

    let onlyOpenFramesIsSumOfFrames (ScoreCard card) =
        let realScore = scoreFromList card
        let expectedScore = card |> List.sum
        realScore |> should equal expectedScore

    [<Test>]
    let ``A score card with only open frames should yield a score equal to the sum of all knocked downcast pins``() =
        Check.QuickThrowOnFailure (Prop.forAll openCardGenerator onlyOpenFramesIsSumOfFrames)


