module FileUpload

open System
open System.IO

let loadTextFromFile (filePath: string) =
    try
        File.ReadAllText(filePath)
    with
    | :? FileNotFoundException -> "File not found."
    | ex -> sprintf "An error occurred: %s" ex.Message


