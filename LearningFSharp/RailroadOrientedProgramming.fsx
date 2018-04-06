open System.IO

type GenerationDataHeaderError = 
    | GenerationIdInvalidFormat
    | GenerationDimensionsInvalidFormat
    | GenerationDimensionsOutOfBound
type ActualLineCount = ActualLineCount of int
type LineIndex = LineIndex of int
type ColumnCount = ColumnCount of int
type GenerationError =
    /// File IO Error
    | FileNotFound of string
    /// Validation Errors
    | InvalidGenerationDataHeader of GenerationDataHeaderError
    | MissingLines of ActualLineCount
    | MissingColumns of LineIndex
    | InvalidCharacter of char
type GenerationData = GenerationData of string list
type FileLineReader = string -> Result<GenerationData,GenerationError>
type GenerationDataValidator = GenerationData -> Result<GenerationData,GenerationError>
type GenerationDataSanitizer = GenerationData -> GenerationData
type GenerationDataLineSanitizer = string -> string
type GenerationHeader = { Id: int; LineCount: int; RowCount: int }
type GenerationInfo = { Header: GenerationHeader; Lines: string list }
type GenerationHeaderReader = GenerationData -> GenerationHeader
type GenerationInfoReader = GenerationData -> GenerationInfo

type CellState = Dead | Alive
type Generation = { Id: int; Cells: CellState [,] }
type GenerationReader = GenerationInfo -> Generation

