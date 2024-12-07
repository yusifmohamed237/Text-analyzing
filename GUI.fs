﻿open System
open System.IO
open System.Drawing
open System.Windows.Forms

[<EntryPoint>]
let main _ =
  
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

    
    loadButton.Click.Add(fun _ ->
        let openFileDialog = new OpenFileDialog(Filter = "Text Files|*.txt")
        if openFileDialog.ShowDialog() = DialogResult.OK then
            inputTextBox.Text <- File.ReadAllText(openFileDialog.FileName)
    )

    
    analyzeButton.Click.Add(fun _ ->
        let text = inputTextBox.Text
        if String.IsNullOrWhiteSpace(text) then
            MessageBox.Show("Please enter or load text to analyze.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning) |> ignore
        else
            resultsBox.Items.Clear()

            let words = text.Split([|' '; '\n'; '\r'; '\t'|], StringSplitOptions.RemoveEmptyEntries)
            let sentences = text.Split([|'.'; '!'; '?'|], StringSplitOptions.RemoveEmptyEntries)
            let paragraphs = text.Split([|"\n\n"|], StringSplitOptions.RemoveEmptyEntries)

            let wordFrequency = 
                words 
                |> Seq.groupBy id 
                |> Seq.map (fun (word, instances) -> word, Seq.length instances)
                |> Seq.sortByDescending snd
                |> Seq.toArray

           
            resultsBox.Items.Add(sprintf "Word Count: %d" words.Length) |> ignore
            resultsBox.Items.Add(sprintf "Sentence Count: %d" sentences.Length) |> ignore
            resultsBox.Items.Add(sprintf "Paragraph Count: %d" paragraphs.Length) |> ignore
            resultsBox.Items.Add(sprintf "Average Sentence Length: %.2f words" (float words.Length / float sentences.Length)) |> ignore
            resultsBox.Items.Add("Top 5 Words:") |> ignore
            for (word, count) in wordFrequency |> Seq.take 5 do
                resultsBox.Items.Add(sprintf " - %s: %d times" word count) |> ignore
    )

    
    form.Controls.Add(resultsBox)
    form.Controls.Add(buttonPanel)
    form.Controls.Add(inputTextBox)
    form.Controls.Add(headerPanel)

    Application.Run(form)
    0