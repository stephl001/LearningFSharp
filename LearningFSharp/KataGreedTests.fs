namespace LearningFSharp.Tests

module KataGreedTests =
    open LearningFSharp.KataGreed
    open FsUnit
    open NUnit.Framework
    open FsCheck

    module List = 
        let rec insertions x = function
            | []             -> [[x]]
            | (y :: ys) as l -> (x::l)::(List.map (fun x -> y::x) (insertions x ys))

        let rec permutations = function
            | []      -> seq [ [] ]
            | x :: xs -> Seq.concat (Seq.map (insertions x) (permutations xs))
    

    let allPermutationsYieldSameResult (roll:D6 list) = 
        List.permutations roll 
        |> Seq.choose Roll.create 
        |> Seq.map getScore
        |> Seq.distinct
        |> Seq.length
        |> should equal 1
        
    [<Test>]
    let ``Make sure all permutations of a given throw yield the same results``() =   
        Check.One ({ Config.Quick with EndSize = 6; StartSize=3}, allPermutationsYieldSameResult)

    [<Test>]
    let ``Make sure scores double from triple match to six of a kind``() =
        let tripleToSixShouldDouble (d:D6) =
            let dieValue = 
                match d with
                | One -> 10 | Two -> 2 | Three -> 3 | Four -> 4 | Five -> 5 | Six -> 6
            let replicateDice count = 
                List.replicate count d
            let allScores = 
                [3;4;5;6] |> List.choose (replicateDice >> Roll.create) |> List.map getScore
            let factors = 
                dieValue::allScores 
                |> List.pairwise 
                |> List.map (fun (s1,s2) -> s2/s1)

            factors = [100;2;2;2]

        [One;Two;Three;Four;Five;Six] 
        |> List.forall tripleToSixShouldDouble
        |> should be True


    

