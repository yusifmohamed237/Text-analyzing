// Function to count paragraphs (assuming paragraphs are separated by double newlines)
let countParagraphs (text: string) =
    let paragraphs = text.Split([| "\n\n" |], StringSplitOptions.RemoveEmptyEntries)
    paragraphs.Length

// Function to count word frequency
let wordFrequency (text: string) =
    let words = text.Split([|' '; '\t'; '\n'; '\r'; '.'; ','; ';'; ':'; '!'|], StringSplitOptions.RemoveEmptyEntries)
    words
    |> Array.fold (fun acc word ->
        let word = word.ToLower()
        if acc.ContainsKey(word) then
            acc.[word] <- acc.[word] + 1
        else
            acc.Add(word, 1)
        acc
    ) (System.Collections.Generic.Dictionary<string, int>())
