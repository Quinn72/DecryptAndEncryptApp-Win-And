namespace Enc_Dcry_All_Sys_Build;

public partial class VigenereCipher : ContentPage
{
    public VigenereCipher()
    {
        InitializeComponent();
        ModePicker.SelectedIndex = 0;
    }

    private void OnProcessClicked(object sender, EventArgs e)
    {
        string input = InputText.Text ?? "";
        string keyword = KeywordText.Text ?? "";

        if (string.IsNullOrWhiteSpace(keyword))
        {
            OutputLabel.Text = "Keyword must not be empty.";
            return;
        }

        string key = GenerateKey(input, keyword);
        string result = ModePicker.SelectedIndex == 0
            ? CipherText(input, key)
            : OriginalText(input, key);

        OutputLabel.Text = result;
    }

    private string GenerateKey(string input, string keyword)
    {
        string filteredKeyword = new string(keyword.Where(char.IsLetter).Select(char.ToUpper).ToArray());
        string key = "";
        int j = 0;

        foreach (char c in input)
        {
            if (char.IsLetter(c))
            {
                key += filteredKeyword[j % filteredKeyword.Length];
                j++;
            }
            else
            {
                key += c;
            }
        }

        return key;
    }

    private string CipherText(string input, string key)
    {
        string result = "";
        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];
            if (char.IsLetter(c))
            {
                char baseChar = char.IsUpper(c) ? 'A' : 'a';
                int shift = key[i] - 'A';
                char encrypted = (char)((char.ToUpper(c) - 'A' + shift) % 26 + baseChar);
                result += encrypted;
            }
            else
            {
                result += c;
            }
        }
        return result;
    }

    private string OriginalText(string cipher, string key)
    {
        string result = "";
        for (int i = 0; i < cipher.Length; i++)
        {
            char c = cipher[i];
            if (char.IsLetter(c))
            {
                char baseChar = char.IsUpper(c) ? 'A' : 'a';
                int shift = key[i] - 'A';
                char decrypted = (char)((char.ToUpper(c) - 'A' - shift + 26) % 26 + baseChar);
                result += decrypted;
            }
            else
            {
                result += c;
            }
        }
        return result;
    }
    private async void CopyOutputButton_Clicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(OutputLabel.Text))
        {
            await Clipboard.Default.SetTextAsync(OutputLabel.Text);
            await DisplayAlert("Copied", "Output copied to clipboard.", "OK");
        }
        else
        {
            await DisplayAlert("No Output", "There's nothing to copy yet.", "OK");
        }
    }


}