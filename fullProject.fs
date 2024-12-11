open System
open System.IO
open System.Windows.Forms
open System.Drawing
open System.Text.RegularExpressions
open System.Threading


// Windows Forms UI
let form = new Form(Text = "Text Analyzer", Width = 700, Height = 600, StartPosition = FormStartPosition.CenterScreen, BackColor = Color.LightGray)

let headerPanel = new Panel(Height = 50, Dock = DockStyle.Top, BackColor = Color.BlueViolet)
let headerLabel = new Label(Text = "Text Analyzer", Dock = DockStyle.Fill, ForeColor = Color.Black, Font = new Font("Arial", 16.0f, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter)
headerPanel.Controls.Add(headerLabel)

let inputTextBox = new TextBox(Multiline = true, Dock = DockStyle.Top, Height = 200, Font = new Font("Consolas", 16.0f), BackColor = Color.Beige, ScrollBars = ScrollBars.Vertical)

let buttonPanel = new Panel(Height = 50, Dock = DockStyle.Top, BackColor = Color.Gainsboro)
let loadButton = new Button(Text = "Load File", Width = 100, Height = 30, BackColor = Color.SkyBlue, FlatStyle = FlatStyle.Flat, Location = Point(10, 10))
let analyzeButton = new Button(Text = "Analyze", Width = 100, Height = 30, BackColor = Color.LightGreen, FlatStyle = FlatStyle.Flat, Location = Point(120, 10))
buttonPanel.Controls.Add(loadButton)
buttonPanel.Controls.Add(analyzeButton)

let resultsBox = new ListBox(Dock = DockStyle.Fill, Font = new Font("Arial", 13.0f), BackColor = Color.Beige)

form.Controls.Add(resultsBox)
form.Controls.Add(buttonPanel)
form.Controls.Add(inputTextBox)
form.Controls.Add(headerPanel)

let openFileDialog = new OpenFileDialog() 
// Function to count words
let countWords (text: string) =
    let words = text.Split([|' '; '\t'; '\n'; '\r'; '.'; ','; ';'; ':'; '!'|], StringSplitOptions.RemoveEmptyEntries)
    words.Length

// Function to count sentences
let countSentences (text: string) =
    let regex = new Regex("[.!?]")
    regex.Matches(text).Count

// Function to count paragraphs
let countParagraphs (text: string) =
    let paragraphs = text.Split([| "\n"; "\r\n" |], StringSplitOptions.RemoveEmptyEntries)
    paragraphs.Length

// Function to count word frequency
let wordFrequency (text: string) =
    let words = text.Split([|' '; '\t'; '\n'; '\r'; '.'; ','; ';'; ':'; '!'|], StringSplitOptions.RemoveEmptyEntries)
    words
    |> Array.fold (fun (acc: System.Collections.Generic.Dictionary<string, int>) word ->
        let word = word.ToLower()
        if acc.ContainsKey(word) then
            acc.[word] <- acc.[word] + 1
        else
            acc.Add(word, 1)
        acc
    ) (System.Collections.Generic.Dictionary<string, int>())



// Function to calculate average sentence length
let averageSentenceLength (text: string) =
    let sentences = text.Split([| "."; "!"; "?" |], StringSplitOptions.RemoveEmptyEntries)
    let totalWords = countWords text
    let totalSentences = sentences.Length
    if totalSentences > 0 then
        float totalWords / float totalSentences
    else
        0.0

// Function to display results
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

    // Print results
    printfn "Text Analysis Results:"
    printfn "----------------------"
    printfn "\nWord Count: %d" wordCount
    printfn "\nSentence Count: %d" sentenceCount
    printfn "\nParagraph Count: %d" paragraphCount
    printfn "\nAverage Sentence Length: %.2f words" avgSentenceLength
    printfn "\nTop 5 Most Frequent Words:"
    mostFrequentWords |> Seq.iter (fun kvp -> printfn "%s: %d" kvp.Key kvp.Value)



// Load Button Event: Opens file dialog to load a file
loadButton.Click.Add(fun _ ->
    let result = openFileDialog.ShowDialog()
    if result = DialogResult.OK then
        let filePath = openFileDialog.FileName
        let text = File.ReadAllText(filePath)
        inputTextBox.Text <- text
)

analyzeButton.Click.Add(fun _ ->
    let text = inputTextBox.Text
    if text <> "" then
        // Perform analysis in a background thread
        ThreadPool.QueueUserWorkItem(fun _ ->
            let wordCount = countWords text
            let sentenceCount = countSentences text
            let paragraphCount = countParagraphs text
            let avgSentenceLength = averageSentenceLength text
            let wordFreq = wordFrequency text

            // Generate individual outputs for results
            let outputs = [
                sprintf "Word Count: %d" wordCount
                sprintf "Sentence Count: %d" sentenceCount
                sprintf "Paragraph Count: %d" paragraphCount
                sprintf "Average Sentence Length: %.2f words" avgSentenceLength
                sprintf "\nTop 5 Most Frequent Words:"
            ]

            // Get the top 5 most frequent words
            let mostFrequentWords =
                wordFreq
                |> Seq.sortByDescending (fun kvp -> kvp.Value)
                |> Seq.take 5
                |> Seq.map (fun kvp -> sprintf "%s: %d" kvp.Key kvp.Value)

            // Safely update the UI
            form.Invoke(fun _ ->
                resultsBox.Items.Clear()
                outputs |> List.iter (fun line -> resultsBox.Items.Add(line) |> ignore)
                mostFrequentWords |> Seq.iter (fun word -> resultsBox.Items.Add(word) |> ignore)
            ) |> ignore
        ) |> ignore
)


// Start the application
[<STAThread>]
do Application.Run(form)
