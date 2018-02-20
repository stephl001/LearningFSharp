namespace LearningFSharp

module Palindrome =
    open System

    let private toLower (s:string) = s.ToLowerInvariant()
    let private onlyLetters (s:string) = 
        Seq.filter Char.IsLetter s |> String.Concat
    
    let private sanitize = toLower >> onlyLetters

    let private insideOut (s:string) =
        seq {
            let charArray = s |> Array.ofSeq
            for i in [0 .. s.Length/2-1] do
                let r = (charArray.[i],charArray.[s.Length-i-1])
                printfn "%A" r
                yield r
        }

    let private isStringSameAsReverse = insideOut >> Seq.forall (fun (x,y) -> x=y)

    let isPalindrome = sanitize >> isStringSameAsReverse

