type LoggingBuilder() =
    let log = printfn "expression is %A"
    member __.Bind(x, f) = 
        log x
        f x
    member __.Return(x) = 
        x

let logging = new LoggingBuilder()

let something =
    logging {
        let! x = 42
        let! y = 42
        let! z = x + y
        return z
    }

type MaybeBuilder() =

    member __.Bind(x, f) = 
        match x with
        | None -> None
        | Some a -> f a

    member __.Return(x) = 
        Some x
   
let maybe = new MaybeBuilder()

let divideBy bottom top =
    if bottom = 0
    then None
    else Some(top/bottom)

let divideByWorkflow init x y z = 
    let res = 
        maybe {
            let! a = init |> divideBy x
            let! b = a |> divideBy y
            let! c = b |> divideBy z
            return c
        }
    Option.iter (printfn "%i") res 
