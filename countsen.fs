let countSentences (text: string) =
    let sentenceEndings = [".", "!", "?"]
    let regex = new Regex("[.!?]")
    regex.Matches(text).Count

