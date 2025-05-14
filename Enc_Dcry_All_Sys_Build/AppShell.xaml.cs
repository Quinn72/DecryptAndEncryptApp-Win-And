namespace Enc_Dcry_All_Sys_Build
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(CeasarCipher), typeof(CeasarCipher));
            Routing.RegisterRoute(nameof(VigenereCipher), typeof(VigenereCipher));

        }
    }
}
