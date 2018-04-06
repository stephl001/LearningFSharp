namespace LearningFSharp
 
module ReadFileSample =
 
    type FileReadError =
        | FileNotFound of string
        | FileWrongFormat
        | FileInvalidLength of int
 
    let readFile = function
    | "good.txt" -> Ok "CONTENT:This is a super file content!"
    | "wrongformat.txt" -> Ok "BAD: This is a bad file content!"
    | filename -> Error (FileNotFound filename)
 
    let readContent (content:string) =
        if content.StartsWith("CONTENT:")
        then Ok (content.Substring(8))
        else Error FileWrongFormat
 
    let transformContent (content:string) =
        content.ToUpper()
 
    let printContent = function
    | Ok content -> printfn "Here is the content: %s" content
    | Error e ->
        match e with
        | FileNotFound f -> printfn "The file %s was not found." f
        | FileWrongFormat -> printfn "The file format is invalid"
        | FileInvalidLength l -> printfn "The expected file length is 45. Actual length is %d" l
 
    let (>>=) twoTrackInput switchFunction =
        Result.bind switchFunction twoTrackInput
    let (>=>) twoTrackInput switchFunction =
        Result.map switchFunction twoTrackInput
 
    let transformFileContent filename = readFile filename >>= readContent >=> transformContent
    let printFileContent = transformFileContent >> printContent