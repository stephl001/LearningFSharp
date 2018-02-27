// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.
#I "../packages/"
#r "FsCheck/lib/net452/FSCheck.dll"
#r "FsUnit/lib/net46/FSUnit.Nunit.dll"
#r "NUnit/lib/net45/nunit.framework.dll"
#load "KataGreed.fs"
open LearningFSharp.KataGreed
open FsCheck
open FsUnit
open NUnit.Framework

module List = 
    let rec insertions x = function
        | []             -> [[x]]
        | (y :: ys) as l -> (x::l)::(List.map (fun x -> y::x) (insertions x ys))

    let rec permutations = function
        | []      -> seq [ [] ]
        | x :: xs -> Seq.concat (Seq.map (insertions x) (permutations xs))

let allPermutationsYieldSameResult d1 d2 d3 d4 d5 d6 = 
    let roll = [d1;d2;d3;d4;d5;d6]
    let len = List.permutations roll 
                |> Seq.choose Roll.create 
                |> Seq.map getScore
                |> Seq.distinct
                |> Seq.length

    Assert.AreEqual (1, len)

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