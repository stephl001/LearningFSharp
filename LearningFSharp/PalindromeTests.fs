namespace LearningFSharp.Tests

module PalindromeTests =
    open LearningFSharp.Palindrome    
    open FsUnit
    open NUnit.Framework

    [<Test>]
    let ``Make sure an empty string is considered as a palindrome``() =
        isPalindrome "" |> should be True

    [<TestCase("kayak")>]
    [<TestCase("KAYAK")>]
    let ``Make sure palindrome detection is case insensitive``(str) =
        isPalindrome str |> should be True

    [<Test>]
    let ``Make sure palindrome detection ignores non-letters characters``() =
        isPalindrome "!ka2y2ak!" |> should be True

    [<TestCase("patate")>]
    [<TestCase("poulet")>]
    let ``Make sure non-palindrome are properly detected``(str) =
        isPalindrome str |> should be False
    

