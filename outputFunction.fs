 printfn "Text Analysis Results:"
    printfn "----------------------"
    printfn "Word Count: %d" wordCount
    printfn "Sentence Count: %d" sentenceCount
    printfn "Paragraph Count: %d" paragraphCount
    printfn "Average Sentence Length: %.2f words" avgSentenceLength
    printfn "\nTop 5 Most Frequent Words:"
    mostFrequentWords |> Seq.iter (fun (word, count) -> printfn "%s: %d" word count)
