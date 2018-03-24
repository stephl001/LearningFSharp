module RomanNumerals

type RomanNumerals = 
    | I | IV | V | IX | X | XL | L | XC | C | CD | D | CM | M 

let romanNumeralValues = [
    M,1000
    CM,900
    D,500
    CD,400
    C,100
    XC,90
    L,50
    XL,40
    X,10
    IX,9
    V,5
    IV,4
    I,1
]

let toRomanNumeral n =
    let rec collect nb = function
        | [] -> []
        | (rn,v)::_ as numeralValues when v <= nb -> rn::collect (nb-v) numeralValues
        | _::rest -> collect nb rest
    let concatStr = String.concat ""

    collect n romanNumeralValues
    |> List.map string
    |> concatStr
