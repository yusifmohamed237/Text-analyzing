let main argv =
    // Example: Load text from a file
    let filePath = "sample.txt" // Replace with the path to a .txt file
    let text = loadTextFromFile filePath
    
    if text <> "" then
        displayResults text
    0 // Return an integer exit code