using System;
using System.Windows.Forms;
using fa.api.security;
using fa.views;
using fa.views.controls;

namespace fa
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);
            Encryptor en = new Encryptor();
            Console.Write(en.EncryptText("myaccount"));
            Application.Run(new Container());
            //Application.Run(new Test());
        }
    }
}
