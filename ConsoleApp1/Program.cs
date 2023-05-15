using System.Text;
using System.Text.RegularExpressions;
using System.Reactive;


ConsoleKey keyToCopyNewLine = ConsoleKey.F11;
string pathIn = @"D:\Programmmieren\Projects\ShrekScript as DC aka\Many brave knigts had attempted to .txt";
string pathOut = @"D:\Programmmieren\Projects\ShrekScript as DC aka\out.txt";
string text = File.ReadAllText(pathIn);

text = text.Replace(Environment.NewLine, " ").Replace("\n", " ").Replace("\r", " ");
text = Regex.Replace(text, @"\s+", " ");
text = InsertNewLines(text, 32);

string[] lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

while (true) {
    if (Console.KeyAvailable) {
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);

        if (keyInfo.Key == keyToCopyNewLine) {
            if (currentLineIndex < lines.Length) {
                Clipboard.SetText(lines[currentLineIndex]);
                Console.WriteLine("Copied to clipboard: " + lines[currentLineIndex]);
                currentLineIndex++;
            }
            else {
                Console.WriteLine("End of text reached.");
            }
        }
    }
}

File.WriteAllText(pathOut, text);



static string InsertNewLines(string text, int charactersPerLine) {
    StringBuilder result = new StringBuilder();
    int lineLength = 0;

    string[] words = text.Split(' ');

    foreach (string word in words) {
        if (lineLength + word.Length > charactersPerLine) {
            result.AppendLine();
            lineLength = 0;
        }

        result.Append(word + " ");
        lineLength += word.Length + 1;
    }

    return result.ToString();
}


