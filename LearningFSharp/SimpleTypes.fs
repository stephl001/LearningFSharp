module SimpleTypes

module SgMarketsTypes = 

    type Gender = Dude | Dudette
    type Person = { FirstName: string; LastName: string; Gender: Gender}
    type Position = 
        | Developer
        | Tester
        | BusinessAnalyst
    type SgTeamMember = 
        | SingleMember of Person*Position
        | Manager of SgTeamMember list

    [<Measure>] type month
    [<Measure>] type EUR
    [<Measure>] type USD
    [<Measure>] type GBP

    type ProductDef = { Duration: int<month>; Value: decimal<USD> }
    
    type Product = 
        | Autocall of AutocallProductFamily
        | ReverseConvertible
    and AutocallProductFamily =
        | PhoenixDouble of ProductDef

    type CurrencyRate<[<Measure>]'u, [<Measure>]'v> = 
        { Rate: float<'u/'v>; Date: System.DateTime}
(*
    let mar1 = System.DateTime(2012,3,1)
    let eurToUsdOnMar1 = {Rate= 1.2<USD/EUR>; Date=mar1 }
    let eurToGbpOnMar1 = {Rate= 0.8<GBP/EUR>; Date=mar1 }

    let tenEur = 10.0<EUR>
    let tenEurInUsd = eurToUsdOnMar1.Rate * tenEur 
*)  

module OtherTypeAndMeasures =

    [<Measure>] type ms

    let waitForSomething (delay:int<ms>) =
        printfn "Waiting for %d milliseconds!" delay

    module NonNegativeInt = 
        type T = NonNegativeInt of int

        let create i = 
            if (i >= 0 )
            then Some (NonNegativeInt i)
            else None

    type Coordinate = { X: NonNegativeInt.T; Y: NonNegativeInt.T }

    let createCoordinate = function
    | (Some x, Some y) -> {X=x; Y=y}
    | _ -> failwith "Invalid coordinates..."

    let printCoordinate (coord:Coordinate) = 
        printfn "Here is the coodinate: %A" coord

            