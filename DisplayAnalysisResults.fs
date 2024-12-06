let displayResults (text: string) =
    // Counts
    let wordCount = countWords text
    let sentenceCount = countSentences text
    let paragraphCount = countParagraphs text
    let avgSentenceLength = averageSentenceLength text
    
    // Word frequencies
    let wordFreq = wordFrequency text
    let mostFrequentWords = 
        wordFreq
        |> Seq.sortByDescending (fun kvp -> kvp.Value)
        |> Seq.take 5  // Display top 5 most frequent words
        