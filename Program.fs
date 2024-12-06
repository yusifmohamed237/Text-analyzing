open System
open System.IO
<<<<<<< HEAD
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
=======
open System.Drawing
open System.Windows.Forms

[<EntryPoint>]
let main _ =
    // إنشاء النافذة الرئيسية
    let form = new Form(Text = "Text Analyzer", Width = 700, Height = 600, StartPosition = FormStartPosition.CenterScreen, BackColor = Color.LightGray)

    // إنشاء لوحة علوية
    let headerPanel = new Panel(Height = 50, Dock = DockStyle.Top, BackColor = Color.BlueViolet)
    let headerLabel = new Label(Text = "Text Analyzer", Dock = DockStyle.Fill, ForeColor = Color.Black, Font = new Font("Arial", 16.0f, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter)
    headerPanel.Controls.Add(headerLabel)

    // مربع نص لإدخال النص أو عرض النص المحمّل
    let inputTextBox = new TextBox(Multiline = true, Dock = DockStyle.Top, Height = 200, Font = new Font("Consolas", 16.0f), BackColor = Color.LightGoldenrodYellow, ScrollBars = ScrollBars.Vertical)

    // لوحة للأزرار
    let buttonPanel = new Panel(Height = 50, Dock = DockStyle.Top, BackColor = Color.Gainsboro)
    let loadButton = new Button(Text = "Load File", Width = 100, Height = 30, BackColor = Color.SkyBlue, FlatStyle = FlatStyle.Flat, Location = Point(10, 10))
    let analyzeButton = new Button(Text = "Analyze", Width = 100, Height = 30, BackColor = Color.LightGreen, FlatStyle = FlatStyle.Flat, Location = Point(120, 10))
    buttonPanel.Controls.Add(loadButton)
    buttonPanel.Controls.Add(analyzeButton)

    // قائمة لعرض النتائج
    let resultsBox = new ListBox(Dock = DockStyle.Fill, Font = new Font("Arial", 13.0f), BackColor = Color.LightGray)

    // إضافة حدث لتحميل الملف
    loadButton.Click.Add(fun _ ->
        let openFileDialog = new OpenFileDialog(Filter = "Text Files|*.txt")
        if openFileDialog.ShowDialog() = DialogResult.OK then
            inputTextBox.Text <- File.ReadAllText(openFileDialog.FileName)
    )

    // إضافة حدث لتحليل النص
    analyzeButton.Click.Add(fun _ ->
        let text = inputTextBox.Text
        if String.IsNullOrWhiteSpace(text) then
            MessageBox.Show("Please enter or load text to analyze.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning) |> ignore
        else
            resultsBox.Items.Clear()

            // تحليل النص
            let words = text.Split([|' '; '\n'; '\r'; '\t'|], StringSplitOptions.RemoveEmptyEntries)
            let sentences = text.Split([|'.'; '!'; '?'|], StringSplitOptions.RemoveEmptyEntries)
            let paragraphs = text.Split([|"\n\n"|], StringSplitOptions.RemoveEmptyEntries)

            let wordFrequency = 
                words 
                |> Seq.groupBy id 
                |> Seq.map (fun (word, instances) -> word, Seq.length instances)
                |> Seq.sortByDescending snd
                |> Seq.toArray

            // عرض النتائج
            resultsBox.Items.Add(sprintf "Word Count: %d" words.Length) |> ignore
            resultsBox.Items.Add(sprintf "Sentence Count: %d" sentences.Length) |> ignore
            resultsBox.Items.Add(sprintf "Paragraph Count: %d" paragraphs.Length) |> ignore
            resultsBox.Items.Add(sprintf "Average Sentence Length: %.2f words" (float words.Length / float sentences.Length)) |> ignore
            resultsBox.Items.Add("Top 5 Words:") |> ignore
            for (word, count) in wordFrequency |> Seq.take 5 do
                resultsBox.Items.Add(sprintf " - %s: %d times" word count) |> ignore
    )

    // إضافة عناصر إلى النافذة
    form.Controls.Add(resultsBox)
    form.Controls.Add(buttonPanel)
    form.Controls.Add(inputTextBox)
    form.Controls.Add(headerPanel)

    // تشغيل التطبيق
    Application.Run(form)
    0
>>>>>>> bc20e6eb2fd3a56fe4b8503b790bea45444d720d
