let averageSentenceLength (text: string) =
    let sentences = text.Split([| "."; "!"; "?" |], StringSplitOptions.RemoveEmptyEntries)
    let totalWords = countWords text
    let totalSentences = sentences.Length
    if totalSentences > 0 then
        float totalWords / float totalSentences
    else
        0.0