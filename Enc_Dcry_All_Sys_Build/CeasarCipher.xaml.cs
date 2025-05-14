using System.Text;

namespace Enc_Dcry_All_Sys_Build;

public partial class CeasarCipher : ContentPage
{
    public CeasarCipher()
    {
        InitializeComponent();
    }

    public void OnEncryptClicked(object sender, EventArgs e)
    {
        if (!int.TryParse(CesaerShift.Text, out int shift))
        {
            OutputString.Text = "Invalid Caesar shift value!";
            return;
        }

        string text = InputString.Text;
        text = CaesarEncrypt(text, shift);
        text = Atbash(text);
        text = Rot13(text);
        text = Base64Encode(text);

        OutputString.Text = text;
        SemanticScreenReader.Announce(OutputString.Text);
    }

    public void OnDecryptClicked(object sender, EventArgs e)
    {
        if (!int.TryParse(CesaerShift.Text, out int shift))
        {
            OutputString.Text = "Invalid Caesar shift value!";
            return;
        }

        string text = InputString.Text;
        text = Base64Decode(text);     // Base64 decode is not symmetric, so apply first
        text = Rot13(text);            // Symmetric
        text = Atbash(text);           // Symmetric
        text = CaesarDecrypt(text, shift); // Final reverse Caesar

        OutputString.Text = text;
        SemanticScreenReader.Announce(OutputString.Text);
    }


    private static string CaesarEncrypt(string input, int shift)
    {
        StringBuilder output = new StringBuilder(input.Length);

        foreach (char c in input)
        {
            if (char.IsUpper(c))
            {
                output.Append((char)((((c - 'A') + shift + 26) % 26) + 'A'));
            }
            else if (char.IsLower(c))
            {
                output.Append((char)((((c - 'a') + shift + 26) % 26) + 'a'));
            }
            else
            {
                output.Append(c);
            }
        }

        return output.ToString();
    }

    private static string CaesarDecrypt(string input, int shift)
    {
        return CaesarEncrypt(input, -shift);
    }

    private static string Rot13(string input)
    {
        return CaesarEncrypt(input, 13);
    }

    private static string Atbash(string input)
    {
        StringBuilder output = new StringBuilder(input.Length);

        foreach (char c in input)
        {
            if (char.IsUpper(c))
            {
                output.Append((char)('Z' - (c - 'A')));
            }
            else if (char.IsLower(c))
            {
                output.Append((char)('z' - (c - 'a')));
            }
            else
            {
                output.Append(c);
            }
        }

        return output.ToString();
    }

    private static string Base64Encode(string input)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(input);
        return Convert.ToBase64String(bytes);
    }
    private static string Base64Decode(string input)
    {
        try
        {
            byte[] bytes = Convert.FromBase64String(input);
            return Encoding.UTF8.GetString(bytes);
        }
        catch
        {
            return "[Invalid Base64]";
        }
    }

    private async void OnCopyClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(OutputString.Text))
        {
            try
            {
                await Clipboard.Default.SetTextAsync(OutputString.Text);
                await DisplayAlert("Copied", "Text Copied To Clipboard", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Clipboard Error", $"Failed to copy: {ex.Message}", "OK");
            }
        }
    }
}