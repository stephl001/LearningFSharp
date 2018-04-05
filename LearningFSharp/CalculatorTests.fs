namespace LearningFSharp

module CalculatorTests =
    open Calculator
    open FsUnit
    open NUnit.Framework
    open FsCheck
    open FsCheck.NUnit
    
    [<Property>]
    let ``Addition should be associative`` x y =
        let result1 = add x y
        let result2 = add y x
        result1 = result2

    [<Property>]
    let ``Adding 1 twice is the same as adding 2`` x =
        let result1 = x |> add 1 |> add 1
        let result2 = x |> add 2 
        result1 = result2

    [<Property>]
    let ``Adding zero is the same as doing nothing`` x =
        let result1 = x |> add 0
        result1 = x