open System
open System.IO
open System.Collections.Generic

// Function to load text from a file
let loadTextFromFile (filePath: string) =
    if File.Exists(filePath) then
        File.ReadAllText(filePath)
    else
        printfn "File not found: %s" filePath
        ""

// Function to count paragraphs (assuming paragraphs are separated by double newlines)
let countParagraphs (text: string) =
    let paragraphs = text.Split([| "\n\n" |], StringSplitOptions.RemoveEmptyEntries)
    paragraphs.Length

// Function to count word frequency
let wordFrequency (text: string) =
    let words = text.Split([|' '; '\t'; '\n'; '\r'; '.'; ','; ';'; ':'; '!'|], StringSplitOptions.RemoveEmptyEntries)
    words
    |> Array.fold (fun (acc: Dictionary<string, int>) word ->
        let word = word.ToLower()
        if acc.ContainsKey(word) then
            acc.[word] <- acc.[word] + 1
        else
            acc.Add(word, 1)
        acc
    ) (new Dictionary<string, int>())

// Function to display analysis results
let displayResults (text: string) =
    // Count paragraphs
    let paragraphCount = countParagraphs text

    // Calculate word frequency
    let wordFreq = wordFrequency text
    let mostFrequentWords = 
        wordFreq
        |> Seq.sortByDescending (fun kvp -> kvp.Value)
        |> Seq.take 5

    // Output the results
    printfn "Text Analysis Results:"
    printfn "----------------------"
    printfn "Paragraph Count: %d" paragraphCount
    printfn "\nTop 5 Most Frequent Words:"
    mostFrequentWords |> Seq.iter (fun kvp -> printfn "%s: %d" kvp.Key kvp.Value)

// Main function
[<EntryPoint>]
let main argv =
    let filePath = "sample.txt" // Replace with the path to a .txt file
    let text = loadTextFromFile filePath

    if text <> "" then
        displayResults text
    0 // Return an integer exit code
