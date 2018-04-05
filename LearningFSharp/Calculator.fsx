#load "Calculator.fs"
open LearningFSharp.Calculator

let associativity a b =
    add a b = add b a

